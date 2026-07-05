using Application.Commands;
using Application.DTO;
using Domain.Authorization;
using FluentValidation;
using Implementation.UseCases.Validators;
using Supportly.DataAccess;
using System;
using System.Linq;

namespace Implementation.UseCases.Commands
{
    public class EfUpdateUserCommand : EfUseCase, IUpdateUserCommand
    {
        private readonly UpdateUserValidator _validator;

        public EfUpdateUserCommand(LabDbContext context, UpdateUserValidator validator) : base(context)
        {
            _validator = validator;
        }

        public string Name => "Update user";

        public string Id => UseCaseIds.UpdateUser;

        public void Execute(UserUpdateDTO data)
        {
            _validator.ValidateAndThrow(data);

            var user = ctx.Users.First(u => u.Id == data.Id);   // validator garantuje da postoji

            user.FullName = data.FullName;
            user.Email = data.Email;
            user.Phone = data.Phone;
            user.RoleId = (byte)data.RoleId;
            user.DepartmentId = (short?)data.DepartmentId;
            user.IsActive = data.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            ctx.SaveChanges();
        }
    }
}
