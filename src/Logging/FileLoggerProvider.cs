using ErikTheCoder.Logging.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace ErikTheCoder.Logging;


public sealed class FileLoggerProvider(IOptions<FileLoggerOptions> options) : ILoggerProvider
{
    private FileLogger _fileLogger = new(options);
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
        }

        // Free unmanaged resources.
        _fileLogger?.Dispose();
        _fileLogger = null;

        _disposed = true;
    }

    // Inject singleton FileLogger instance into each FileLoggerDecorator instance.
    // The .NET runtime sets category = T when an ILogger<T> instance writes log messages.
    public ILogger CreateLogger(string categoryName) => new FileLoggerDecorator(_fileLogger, categoryName);
}