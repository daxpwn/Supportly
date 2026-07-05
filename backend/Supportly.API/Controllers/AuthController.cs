using Supportly.API.DTO;
using Supportly.API.JWT;
using Supportly.DataAccess;
using Domain;
using Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.PortableExecutable;

/*
    admin@supportly.rs	Admin123!	admin
    agent@supportly.rs	Agent123!	agent
    klijent@supportly.rs	Klijent123!	customer
*/

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Supportly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private LabDbContext _context;
        private JwtHandler _handler;

        public AuthController(LabDbContext context, JwtHandler handler)
        {
            _context = context;
            _handler = handler;
        }

        //Korisnik se loguje slanjem username/password -> dobije token i refresh token (traje dugo)
        //Token dostavlja svakim requestom
        //Token istice
        //Upotrebom refresh tokena, korisnik dobija novi token i novi refresh token - invalidacija upotrebljenog refresh tokena
        //Logout sa svih uredjaja - invalidacija svih tokena i svih refresh tokena
        // POST api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            User user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if(user == null)
            {
                return Unauthorized();
            }

            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized();
            }

            return Ok(_handler.MakeToken(user));
        }

        
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if(!Request.Headers.ContainsKey("Authorization"))
            {
                return NotFound();
            }

            var header = Request.Headers["Authorization"];

            var headerParts = header.ToString().Split(" ");

            if (headerParts.Count() != 2 || headerParts[0] != "Bearer")
            {
                return NotFound();
            }

            var token = headerParts[1];

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            
            string tokenId = jwtToken.Claims.FirstOrDefault(x => x.Type == "TokenId").Value;

            //Pronaci originalni token koji se invaldiira
            //Pronaci njegov refresh token
            //Invalidirati oba

            AuthToken jwt = _context.AuthTokens
                                   .Include(x => x.RefreshToken)
                                   .FirstOrDefault(x => x.TokenId == tokenId);

            if (jwt == null) 
            {
                return NotFound();
            }

            var now = DateTime.UtcNow;

            if(!jwt.InvalidatedAt.HasValue)
            {
                jwt.InvalidatedAt = now;
            }

            if(!jwt.RefreshToken.InvalidatedAt.HasValue)
            {
                jwt.RefreshToken.InvalidatedAt = now;
            }

            _context.SaveChanges();

            return NoContent();
        }


        /*
            - Stize refresh token - kroz body
            - Da li postoji refresh token? - Ne postoji -> 404
                - Da li je istekao -> istekao -> 401
                - Da li invalidiran -> invalidiran -> 401
                - Invalidacija jwt-a (samo onog za koji je refresh token vezan)
                - Invalidacija refresh tokena
                - Generisemo novi jwt 
                - Generisemo novi refresh
                - Vracamo 200 uz tokene
        */
        // POST api/auth/refresh
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshTokenRequest request)
        {
            var refreshToken = _context.AuthTokens
                                       .Include(x => x.JwtToken)
                                       .Include(x => x.User)
                                       .FirstOrDefault(x => x.TokenId == request.RefreshToken);
            
            if(refreshToken == null)
            {
                return NotFound();
            }

            if(DateTime.UtcNow > refreshToken.ExpiresAt)
            {
                return Unauthorized();
            }

            if(refreshToken.InvalidatedAt.HasValue)
            {
                return Unauthorized();
            }

            refreshToken.JwtToken.InvalidatedAt = DateTime.UtcNow;
            refreshToken.InvalidatedAt = DateTime.UtcNow;

            return Ok(_handler.MakeToken(refreshToken.User));
        }
    }
}
