using System;

namespace Supportly.API.ExceptionLogging
{
    public interface IExceptionLogger
    {
        Guid Log(Exception ex);
    }
}
