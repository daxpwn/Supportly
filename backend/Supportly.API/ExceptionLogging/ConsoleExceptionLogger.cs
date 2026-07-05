using System;

namespace Supportly.API.ExceptionLogging
{
    public class ConsoleExceptionLogger : IExceptionLogger
    {
        public Guid Log(Exception ex)
        {
            Guid logId = Guid.NewGuid();

            Console.WriteLine("An error occurred at:"  + DateTime.UtcNow.ToLocalTime());
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);

            return logId;
        }
    }
}
