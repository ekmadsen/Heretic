using ErikTheCoder.Heretic.Data.Extensions;
using ErikTheCoder.Heretic.WebApi.Extensions;
using ErikTheCoder.Identity.Extensions;
using ErikTheCoder.Logging.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// TODO: Refactor all configuration extensions methods so the configureOptions parameter is optional.
builder.Logging
    .ClearProviders()
    .AddDebug()
    .AddFile(options => builder.Configuration.GetSection("Logger").Bind(options))
    .AddDatabase(options =>
    {
        builder.Configuration.GetSection("Logger").Bind(options);
        options.Connection = builder.Configuration.GetConnectionString("ApplicationLogsDatabase");
    });

builder.Services
    .AddJwtAuthentication(options => builder.Configuration.GetSection("Identity").Bind(options))
    .AddPolicyBasedAuthorization()
    .AddDatabases(builder.Configuration)
    .AddIdentity(options => builder.Configuration.GetSection("Identity").Bind(options))
    .AddSwaggerGen(options =>
    {
        // Add JWT security scheme.
        const string jwt = "JWT";
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            Name = jwt,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Enter the token.",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http
        };
        options.AddSecurityDefinition(jwt, jwtSecurityScheme);

        // Require JWT security scheme.
        var requiredSecurityScheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Id = jwt,
                Type = ReferenceType.SecurityScheme
            }
        };
        var securityRequirement = new OpenApiSecurityRequirement
        {
            [requiredSecurityScheme] = []
        };
        options.AddSecurityRequirement(securityRequirement);
    })
    .AddControllers();

var application = builder.Build();

application
    .UseHttpsRedirection()
    .UseAuthentication() // AuthN: Determine identity of calling client.
    .UseAuthorization()  // AuthZ: Restrict what the identified client is permitted to do.  Must appear between UseAuthentication() and MapControllers().
    .UseAuthorization()
    .UseSwagger()
    .UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });

application.MapControllers().RequireAuthorization(options =>
{
    // Secure REST endpoints.
    options.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

application.Run();