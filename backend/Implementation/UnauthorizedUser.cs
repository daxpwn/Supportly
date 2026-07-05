using Application;
using Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Implementation
{
    public class UnauthorizedUser : IApplicationUser
    {
        public int Id => 0;

        public string Email => "guest@ict.edu.rs";

        public string Username => "unauthorized";

        public IEnumerable<string> AllowedUseCases =>
            new List<string> { UseCaseIds.Register, UseCaseIds.GetTickets };
    }
}
