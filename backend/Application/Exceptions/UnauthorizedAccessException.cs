using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Exceptions
{
    public class UnauthorizedUseCaseException : Exception
    {
        public UnauthorizedUseCaseException(string username, string useCaseName)
            : base($"User {username} has tried to execute {useCaseName}.")
        {
            
        }
    }
}
