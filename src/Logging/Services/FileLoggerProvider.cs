using ErikTheCoder.Logging.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace ErikTheCoder.Logging.Services;


public sealed class FileLoggerProvider(IOptions<FileLoggerOptions> options) : ILoggerProvider
{
    private FileLogger _logger = new(options);


    ~FileLoggerProvider() => Dispose(false);


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Free managed resources.
        }

        // Free unmanaged resources.
        _logger?.Dispose();
        _logger = null;
    }

    // Inject singleton FileLogger instance into each FileLoggerDecorator instance.
    // The .NET runtime sets category = T when an ILogger<T> instance writes log messages.
    public ILogger CreateLogger(string categoryName) => new FileLoggerDecorator(_logger, categoryName);
}