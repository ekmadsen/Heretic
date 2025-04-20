using ErikTheCoder.Heretic.Contracts.Internal;


namespace ErikTheCoder.Heretic.WebApi.Extensions;


#pragma warning disable SYSLIB1015
public static partial class LoggingExtensions
{
    [LoggerMessage(EventId = Event.FooBarId, EventName = Event.FooBarName, Level = LogLevel.Information, Message = "Foo = {foo}.  Bar = {bar}.")]
    public static partial void FooBar(this ILogger logger, string foo, string bar);


    [LoggerMessage(EventId = Event.HelloWorldId, EventName = Event.HelloWorldName, Level = LogLevel.Information)]
    public static partial void HelloWorld(this ILogger logger, string baz, string zap);
}