using Supportly.API.Controllers;
using Supportly.API.DTO;
using Supportly.API.JWT;
using Supportly.DataAccess;
using Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using Xunit;

namespace Tests
{
    public class JwtTests
    {
        private LabDbContext ctx;

        public JwtTests()
        {
            ctx = new LabDbContext();
        }
        /*
            Svaki test se sastoji iz:
            1. Postavljanje inicijalnog (poznatog) stanja 
            2. Izvrsavanja testa (algoritam)
            3. Verifikacije rezultata
        */

        private AuthController Controller
        {
            get
            {
                var settings = new Supportly.API.AppSettings
                {
                    JwtSettings = new Supportly.API.JwtSettings
                    {
                        RefreshTokenHours = 154,
                        DurationSeconds = 600,
                        SecretKey = "TEST123143325341234214TEST1232131231231LabASP",
                        Issuer = "Visoka ICT"
                    }
                };
                var jwtHandler = new JwtHandler(settings, ctx);

                var controller = new AuthController(ctx, jwtHandler);
                return controller;
            }
        }

        [Fact]
        public void Returns_404_When_RefreshTokenDoesntExist()
        {
            //Postavljanje podataka
            var dto = new RefreshTokenRequest
            {
                RefreshToken = Guid.NewGuid().ToString()
            };

            var result = Controller.Refresh(dto);

            //Verifikacije rezultata - Assert izrazima
            //Assert.True(result is NotFoundResult);
            (result is NotFoundResult).Should().BeTrue();
        }

        [Fact]
        public void Retursn_401_When_TokenHasExpired()
        {
            var tokenId = Guid.NewGuid().ToString();
            //Initial data
            var authToken = new AuthToken
            {
                ExpiresAt = DateTime.UtcNow.AddSeconds(-3),
                UserId = 1,
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                TokenId = tokenId
            };

            ctx.AuthTokens.Add(authToken);
            ctx.SaveChanges();

            var result = Controller.Refresh(new RefreshTokenRequest { RefreshToken = tokenId });

            result.Should().BeOfType<UnauthorizedResult>();
            ctx.AuthTokens.Remove(authToken);
            ctx.SaveChanges();
        }

        [Fact]
        public void Retursn_401_When_TokenIsInvalidated()
        {
            var tokenId = Guid.NewGuid().ToString();
            //Initial data
            var authToken = new AuthToken
            {
                ExpiresAt = DateTime.UtcNow.AddSeconds(30),
                UserId = 1,
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                TokenId = tokenId,
                InvalidatedAt = DateTime.UtcNow.AddSeconds(-10)
            };

            ctx.AuthTokens.Add(authToken);
            ctx.SaveChanges();

            var result = Controller.Refresh(new RefreshTokenRequest { RefreshToken = tokenId });

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void Returns_NewTokenAndRefreshToken_When_RefreshTokenIsValid()
        {
            var tokenId = Guid.NewGuid().ToString();
            //Initial data
            var authRefreshToken = new AuthToken
            {
                ExpiresAt = DateTime.UtcNow.AddSeconds(30),
                UserId = 1,
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                TokenId = tokenId
            };

            var authJwtToken = new AuthToken
            {
                ExpiresAt = DateTime.UtcNow.AddSeconds(5),
                UserId = 1,
                CreatedAt = DateTime.UtcNow.AddMinutes(-10),
                TokenId = Guid.NewGuid().ToString()
            };

            authRefreshToken.JwtToken = authJwtToken;

            ctx.AuthTokens.Add(authRefreshToken);
            ctx.SaveChanges();

            var result = Controller.Refresh(new RefreshTokenRequest { RefreshToken = tokenId });

            result.Should().BeOfType<OkObjectResult>();

            (result as OkObjectResult).Value.Should().BeOfType<JwtTokenResponse>();

            var tokenResponse = (result as OkObjectResult).Value as JwtTokenResponse;

            tokenResponse.RefreshToken.Should().NotBeNull();
            tokenResponse.Token.Should().NotBeNull();

            //var handler = new JwtSecurityTokenHandler();
            //var jwtToken = handler.CreateJwtSecurityToken(tokenResponse.Token);
        }
    }
}
