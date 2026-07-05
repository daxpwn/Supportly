using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public interface IApplicationUser
    {
        public int Id { get; }
        public string Email { get; }
        public string Username { get; }
        public IEnumerable<string> AllowedUseCases { get; }
    }
}
