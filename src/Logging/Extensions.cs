using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;


namespace ErikTheCoder.Logging;

public static class Extensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>());
        return builder;
    }
}