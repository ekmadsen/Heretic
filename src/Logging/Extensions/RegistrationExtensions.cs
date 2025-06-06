﻿using ErikTheCoder.Logging.Options;
using ErikTheCoder.Logging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using OptionsFactory = Microsoft.Extensions.Options.Options;


namespace ErikTheCoder.Logging.Extensions;


public static class RegistrationExtensions
{
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder, Action<FileLoggerOptions> configureOptions)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>());

        var fileLoggerOptions = new FileLoggerOptions();
        configureOptions(fileLoggerOptions);
        LoggerOptions loggerOptions = fileLoggerOptions;

        builder.Services.AddSingleton(OptionsFactory.Create(loggerOptions));
        builder.Services.AddSingleton(OptionsFactory.Create(fileLoggerOptions));

        return builder;
    }


    public static ILoggingBuilder AddDatabase(this ILoggingBuilder builder, Action<DatabaseLoggerOptions> configureOptions)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DatabaseLoggerProvider>());

        var databaseLoggerOptions = new DatabaseLoggerOptions();
        configureOptions(databaseLoggerOptions);
        LoggerOptions loggerOptions = databaseLoggerOptions;

        builder.Services.AddSingleton(OptionsFactory.Create(loggerOptions));
        builder.Services.AddSingleton(OptionsFactory.Create(databaseLoggerOptions));

        return builder;
    }
}