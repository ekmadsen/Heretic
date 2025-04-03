using Microsoft.Extensions.Logging;


namespace ErikTheCoder.Logging;


public class FileLoggerWithCategory(FileLogger fileLogger, string category) : ILogger, IDisposable
{
    private FileLogger _fileLogger = fileLogger;
    private string _category = category;
    private bool _disposed;


    ~FileLoggerWithCategory() => Dispose(false);


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    private void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Free managed resources.
            _category = null;
        }

        // Free unmanaged resources.
        _fileLogger?.Dispose();
        _fileLogger = null;

        _disposed = true;
    }


    public IDisposable BeginScope<TState>(TState state) where TState : notnull => _fileLogger.BeginScope(state);


    public bool IsEnabled(LogLevel logLevel) => _fileLogger.IsEnabled(logLevel);


    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) =>
        _fileLogger.Log(logLevel, eventId, state, exception, formatter, _category);
}