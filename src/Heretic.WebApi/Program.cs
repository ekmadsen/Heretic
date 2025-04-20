using ErikTheCoder.Heretic.Core.Extensions;
using ErikTheCoder.Heretic.Data.Extensions;
using ErikTheCoder.Heretic.WebApi.Extensions;
using ErikTheCoder.Logging;


var builder = WebApplication.CreateBuilder(args);

builder.Logging
    .ClearProviders()
    .AddDebug()
    .AddFile();

builder.Services
    .AddHereticOptions(builder.Configuration)
    .AddDatabases(builder.Configuration)
    .AddCoreServices()
    .AddSwaggerGen()
    .AddControllers();

var application = builder.Build();

application
    .UseHttpsRedirection()
    .UseAuthorization()
    .UseSwagger()
    .UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });

application.MapControllers();
application.Run();