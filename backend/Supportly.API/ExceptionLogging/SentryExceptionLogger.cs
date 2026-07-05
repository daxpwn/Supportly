using Sentry;
using System;

namespace Supportly.API.ExceptionLogging
{
    public class SentryExceptionLogger : IExceptionLogger
    {
        public Guid Log(Exception ex)
        {
            Guid guid = Guid.NewGuid();
            var id = SentrySdk.CaptureException(ex);
            return guid;
        }
    }
}
