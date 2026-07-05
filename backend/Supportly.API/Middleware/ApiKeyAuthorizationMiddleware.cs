using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Supportly.API.Middleware
{
    public class ApiKeyAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        public ApiKeyAuthorizationMiddleware(RequestDelegate next, AppSettings settings)
        {
            _next = next;
            _appSettings = settings;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            //Ka kom endpointu request ide i da li je pokriven x-api-key autorizacijom
            //context.Request.Path.Value == "/api/actors/5/plays";

            //Provera da li endpoint zahteva ApiKeyAuthorizationAttribute

            var endpoint = context.GetEndpoint();

            if (endpoint == null) 
            { 
                await _next(context);
                return;
            }

            var attribute = endpoint.Metadata.GetMetadata<ApiKeyAuthorizationAttribute>();

            if (attribute == null)
            {
                await _next(context);
                return;
            }

            //Da li je API key prisutan u x-api-key headeru
            //Ako je prisutan - provera da li podrzavamo kljuc (poredimo sa config fajlom)
            //Ako ne podrzavamo kljuc/nema kljuca - vracamo 401

            if (!context.Request.Headers.ContainsKey("x-api-key"))
            {
                context.Response.StatusCode = 401;
                return;
            }

            var apiKey = context.Request.Headers["x-api-key"].ToString();
            
            if(!_appSettings.ApiKeys.Contains(apiKey))
            {
                context.Response.StatusCode = 401;
                return;
            }



            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }
}
