using Application;
using Application.Commands;
using Application.Queries;
using Supportly.API.ExceptionLogging;
using Supportly.API.JWT;
using Supportly.API.Storage;
using Supportly.DataAccess;
using Implementation;
using Implementation.UseCases;
using Implementation.UseCases.Commands;
using Implementation.UseCases.Queries;
using Implementation.UseCases.Validators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Supportly.API
{
    public static class ApiExtensions
    {
        public static bool IsLocal(this IWebHostEnvironment env)
        {
            return env.EnvironmentName == "Development";
        }

        public static void SetupApplication(this IServiceCollection services, AppSettings settings)
        {
            services.AddSingleton(settings);
            services.AddTransient(x => new LabDbContext(settings.ConnString));
            services.AddTransient<IExceptionLogger, SentryExceptionLogger>();
            services.AddTransient<IApplicationUser, UnauthorizedUser>();
            services.AddTransient<IRegisterUserCommand, EfRegisterUserCommand>();
            services.AddTransient<IGetTicketsQuery, EfGetTicketsQuery>();
            services.AddTransient<IGetMyTicketsQuery, EfGetMyTicketsQuery>();
            services.AddTransient<IGetTicketQuery, EfGetTicketQuery>();
            services.AddTransient<IGetUsersQuery, EfGetUsersQuery>();
            services.AddTransient<IGetUserQuery, EfGetUserQuery>();
            services.AddTransient<IUpdateUserCommand, EfUpdateUserCommand>();
            services.AddTransient<IDeleteUserCommand, EfDeleteUserCommand>();
            services.AddTransient<IGetCategoriesQuery, EfGetCategoryQuery>();
            services.AddTransient<IGetDepartmentsQuery, EfGetDepartmentsQuery>();
            services.AddTransient<IGetPrioritiesQuery, EfGetPrioritiesQuery>();
            services.AddTransient<IGetStatusesQuery, EfGetStatusesQuery>();
            services.AddTransient<IGetUseCaseLogsQuery, EfGetUseCaseLogsQuery>();
            services.AddTransient<IGetRolesQuery, EfGetRolesQuery>();
            services.AddTransient<IGetUseCaseCatalogQuery, EfGetUseCaseCatalogQuery>();
            services.AddTransient<IAddRoleUseCaseCommand, EfAddRoleUseCaseCommand>();
            services.AddTransient<IRemoveRoleUseCaseCommand, EfRemoveRoleUseCaseCommand>();
            services.AddTransient<ITicketInsertCommand, EfTicketInsertCommand>();
            services.AddTransient<ITicketInsertCommentCommand, EfInsertTicketComment>();
            services.AddTransient<IChangeTicketStatusCommand, EfChangeTicketStatus>();
            services.AddTransient<IUploadAttachmentCommand, EfUploadAttachmentCommand>();
            services.AddTransient<IFileStorage, DiskFileStorage>();
            services.AddTransient<RegisterUserValidator>();
            services.AddTransient<InsertTicketValidator>();
            services.AddTransient<InsertCommentValidator>();
            services.AddTransient<UploadAttachmentValidator>();
            services.AddTransient<UpdateUserValidator>();
            services.AddTransient<UseCaseHandler>();
            services.AddTransient<JwtHandler>();

            services.AddTransient<IApplicationUser>(container =>
            {
                var accessor = container.GetService<IHttpContextAccessor>(); //service locator

                if(accessor.HttpContext == null)
                {
                    return new UnauthorizedUser();
                }

                if (!accessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    return new UnauthorizedUser();
                }
                
                var header = accessor.HttpContext.Request.Headers.Authorization; //Bearer token
                var headerParts = header.ToString().Split(" ");
                
                if(headerParts.Count() != 2 || headerParts[0] != "Bearer")
                {
                    return new UnauthorizedUser();
                }

                var token = headerParts[1];

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                //jwtToken.Claims

                return new JwtUser
                {
                    Id = int.Parse(jwtToken.Claims.FirstOrDefault(x => x.Type == "Id").Value),
                    Username = jwtToken.Claims.FirstOrDefault(x => x.Type == "FullName").Value,
                    Email = jwtToken.Claims.FirstOrDefault(x => x.Type == "Email").Value,
                    AllowedUseCases = jwtToken.Claims
                                              .Where(x => x.Type == "UseCase")
                                              .Select(x => x.Value)
                                              .ToList(),
                };
            });
        }
    }
}
