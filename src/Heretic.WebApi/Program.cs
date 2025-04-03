using ErikTheCoder.Logging;
using ErikTheCoder.Logging.Settings;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services
    .Configure<FileLoggerSettings>(builder.Configuration.GetSection("Logger"))
    .Configure<LoggerSettings>(builder.Configuration.GetSection("Logger"));

builder.Logging
    .ClearProviders()
    .AddDebug()
    .AddFile();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app
        .UseSwagger()
        .UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
}

app
    .UseHttpsRedirection()
    .UseAuthorization();
app.MapControllers();

app.Run();