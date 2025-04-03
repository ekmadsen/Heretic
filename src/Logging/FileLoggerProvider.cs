using System.Collections.Concurrent;
using ErikTheCoder.Logging.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace ErikTheCoder.Logging;


public sealed class FileLoggerProvider(IOptions<FileLoggerSettings> settings) : ILoggerProvider
{
    private FileLogger _fileLogger = new(settings);
    private ConcurrentDictionary<string, FileLoggerWithCategory> _categorizedFileLoggers = [];
    private bool _disposed;


    ~FileLoggerProvider() => Dispose(false);


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
            _categorizedFileLoggers = null;
        }

        // Free unmanaged resources.
        _fileLogger?.Dispose();
        _fileLogger = null;

        _disposed = true;
    }

    // Create one FileLoggerWithCategory instance per category.
    // Inject the singleton FileLogger instance into each FileLoggerWithCategory instance.
    // The .NET runtime sets category = T when an ILogger<T> instance writes log messages.
    public ILogger CreateLogger(string categoryName) => _categorizedFileLoggers.GetOrAdd(categoryName, category => new FileLoggerWithCategory(_fileLogger, category));
}