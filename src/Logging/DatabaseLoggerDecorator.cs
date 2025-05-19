using Microsoft.Extensions.Logging;


namespace ErikTheCoder.Logging;


public class DatabaseLoggerDecorator(DatabaseLogger logger, string category) : ILogger
{
    public bool IsEnabled(LogLevel logLevel) => logger.IsEnabled(logLevel);


    // TODO: Implement stacked scopes.  When scope is disposed, pop its data off stack.
    // See https://nblumhardt.com/2016/11/ilogger-beginscope/.
    public IDisposable BeginScope<TState>(TState state) where TState : notnull => null;


    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) =>
        logger.Log(logLevel, eventId, state, exception, formatter, category);
}