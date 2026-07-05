using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Sentry;
using Supportly.API;
using Supportly.API.Middleware;
using Supportly.DataAccess;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
Working effectively with legacy code - Michael Feathers 
 
*/

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Cross cutting concerns - pravila koja vaze za svaki slucaj koriscenja
//Autorizacija, Logging, TimeTracking 

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularDev", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var settings = new AppSettings();
builder.Configuration.Bind(settings); //Config objekat popunjem podacima iz config fajla
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Unesi JWT token (bez 'Bearer ' prefiksa)."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
});

builder.Services.SetupApplication(settings);
builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = settings.JwtSettings.Issuer,
        ValidateIssuer = true,
        ValidAudience = "Any",
        ValidateAudience = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtSettings.SecretKey)),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    cfg.Events.OnTokenValidated = context =>
    {
        var dbContext = context.HttpContext.RequestServices.GetRequiredService<LabDbContext>();
        var token = context.HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
        var tokenObj = new JwtSecurityTokenHandler().ReadJwtToken(token);

        var tokenId = tokenObj.Claims.FirstOrDefault(x => x.Type == "TokenId").Value;

        AuthToken dbToken = dbContext.AuthTokens.FirstOrDefault(x => x.TokenId == tokenId);

        if(dbToken == null || dbToken.InvalidatedAt.HasValue)
        {
            context.Fail("Unauthorized");
        }
        
        return Task.CompletedTask;
    };

});

var app = builder.Build();

// Configure the HTTP request pipeline.

var config = app.Services.GetService<AppSettings>();
//TDD - test driven development
//

SentrySdk.Init(options =>
{
    options.Dsn = "https://beae93df038fb46be408d8abf0de7801@o4511455391842305.ingest.de.sentry.io/4511455428673616";
    options.Debug = true;
    // Adds request URL and headers, IP and name for users, etc.
    options.SendDefaultPii = true;
});

if (app.Environment.IsLocal())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    Console.WriteLine("Lokalno okruzenje.");
} else
{
    Console.WriteLine(app.Environment.EnvironmentName);
}



app.UseStaticFiles();   // servira wwwroot (npr. /Uploads/{fajl})
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseCors("AngularDev");
app.UseMiddleware<ApiKeyAuthorizationMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
