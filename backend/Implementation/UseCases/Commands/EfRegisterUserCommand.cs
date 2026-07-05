using Application.Commands;
using Application.DTO;
using Supportly.DataAccess;
using Domain;
using Domain.Authorization;
using FluentValidation;
using Implementation.UseCases.Validators;
using System;
using System.Linq;

namespace Implementation.UseCases.Commands
{
    public class EfRegisterUserCommand : EfUseCase, IRegisterUserCommand
    {
        private readonly RegisterUserValidator _validator;
        public EfRegisterUserCommand(LabDbContext context, RegisterUserValidator validator) : base(context)
        {
            _validator = validator;
        }

        public string Name => "User registration";

        public string Id => UseCaseIds.Register;

        public void Execute(RegisterUserDTO data)
        {
            _validator.ValidateAndThrow(data);

            string hash = BCrypt.Net.BCrypt.HashPassword(data.Password);

            var now = DateTime.UtcNow;

            // Samoregistracija -> uloga "customer" (klijent)
            var customerRole = ctx.Roles.FirstOrDefault(r => r.Name == "customer");

            User user = new User
            {
                FullName = data.FullName,
                Email = data.Email,
                PasswordHash = hash,
                Phone = data.Phone,
                RoleId = customerRole != null ? customerRole.Id : (byte)0,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            };

            ctx.Users.Add(user);
            ctx.SaveChanges();

            // Dozvole ne kopiramo po korisniku — korisnik ih nasleđuje kroz rolu (RoleUseCases).

            //slanje email-a
        }
    }
}
