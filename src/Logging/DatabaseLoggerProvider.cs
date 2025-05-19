using ErikTheCoder.Logging.Contracts;
using ErikTheCoder.Logging.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace ErikTheCoder.Logging;


public sealed class DatabaseLoggerProviderProvider(IOptions<DatabaseLoggerOptions> options, IApplicationLogsRepository repository) : ILoggerProvider
{
    private DatabaseLogger _logger = new(options, repository);


    ~DatabaseLoggerProviderProvider() => Dispose(false);


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

    // Inject singleton DatabaseLogger instance into each DatabaseLoggerDecorator instance.
    // The .NET runtime sets category = T when an ILogger<T> instance writes log messages.
    public ILogger CreateLogger(string categoryName) => new DatabaseLoggerDecorator(_logger, categoryName);
}