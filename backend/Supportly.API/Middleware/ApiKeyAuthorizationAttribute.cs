using System;

namespace Supportly.API.Middleware
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ApiKeyAuthorizationAttribute : Attribute
    {
    }
}
