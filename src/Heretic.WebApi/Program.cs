using ErikTheCoder.Heretic.Data.Extensions;
using ErikTheCoder.Identity.Extensions;
using ErikTheCoder.Logging.Extensions;


var builder = WebApplication.CreateBuilder(args);

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
    .AddDatabases(builder.Configuration)
    .AddIdentity(options => builder.Configuration.GetSection("Identity").Bind(options))
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