using System.Collections.Concurrent;
using ErikTheCoder.Logging.Options;
using ErikTheCoder.Utilities.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace ErikTheCoder.Logging;


public abstract class ConcurrentLoggerBase : ILogger, IDisposable, IAsyncDisposable
{
    private LoggerOptions _options;
    private ConcurrentQueue<LogData> _queue; // Thread-safe.
    private Timer _timer;


    protected ConcurrentLoggerBase(IOptions<LoggerOptions> options)
    {
        _options = options.Value;

        // Create queue to hold log data.
        // This enables zero-latency logging by deferring I/O cost of writing logs to data store.
        _queue = new ConcurrentQueue<LogData>();

        // Create timer that drains queue on an interval, writing logs to data store.
        // Timer calls method on a ThreadPool thread.
        _timer = new Timer(WriteLogs, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(options.Value.QueueIntervalMs));
    }


    ~ConcurrentLoggerBase() => Dispose();


    public void Dispose()
    {
        DisposeInternal();
        GC.SuppressFinalize(this);
    }


    protected virtual void DisposeInternal()
    {
        // Wait for queue to drain.
        while (_queue is { IsEmpty: false })
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(_options?.QueueIntervalMs ?? 0));
        }

        // Free unmanaged resources.
        _timer?.Dispose();
        _timer = null;

        // Free managed resources.
        _options = null;
        _queue = null;
    }


    public async ValueTask DisposeAsync()
    {
        await DisposeInternalAsync();
        GC.SuppressFinalize(this);
    }


    protected virtual async ValueTask DisposeInternalAsync()
    {
        // Wait for queue to drain.
        while (_queue is { IsEmpty: false })
        {
            await Task.Delay(TimeSpan.FromMilliseconds(_options?.QueueIntervalMs ?? 0));
        }
        
        // Free unmanaged resources.
        if (_timer != null) await _timer.DisposeAsync();
        _timer = null;

        // Free managed resources.
        _options = null;
        _queue = null;
    }
    
    
    public bool IsEnabled(LogLevel logLevel) => logLevel >= _options.LogLevel;


    public virtual IDisposable BeginScope<TState>(TState state) where TState : notnull => null; // Must be implemented by decorator.


    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) =>
        Log(logLevel, eventId, state, exception, formatter, null);


    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter, string category)
    {
        if (!IsEnabled(logLevel)) return;

        // Add log data to queue.
        var data = GetLogData(logLevel, eventId, state, exception, formatter, category);
        _queue.Enqueue(data);
    }


    private LogData GetLogData<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter, string category)
    {
        // Create a DTO to hold log data.
        // This is critical because the lifetime of the state parameter is not guaranteed to exceed the interval from client calling ILogger.Log and this code
        //   actually writing the log to a data store.
        //  Microsoft documentation states:
        //   "If your implementation strives to queue logging messages in a non-blocking manner [Erik: as we're doing here via an in-memory queue],
        //   the messages should first be materialized or the object state that's used to materialize a log entry should be serialized.
        //   Doing so avoids potential exceptions from disposed objects."
        // See https://learn.microsoft.com/en-us/dotnet/core/extensions/logging-providers#logging-provider-design-considerations.

        var data = new LogData
        {
            Timestamp = DateTimeOffset.Now,
            Application = _options.Application,
            Component = _options.Component,
            Category = category,
            LogLevel = logLevel,
            EventId = eventId,
            Message = formatter(state, exception),
            Properties = state is IEnumerable<KeyValuePair<string, object>> properties ? properties.ToDictionary() : []
        };

        if (data.Properties.TryGetValue(_options.CorrelationIdPropertyName, out var propertyValue))
        {
            // Attempt to set log correlation ID from a well-known property.
            var correlationId = propertyValue?.ToString();
            data.CorrelationId = correlationId.IsNullOrWhiteSpace() ? null : Guid.Parse(correlationId);
            data.Properties.Remove(_options.CorrelationIdPropertyName);
        }

        return data;
    }


    // Usually async void is an anti-pattern.
    // However, it's appropriate for event handlers.
    // See https://learn.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming.
    // ReSharper disable once AsyncVoidMethod
    private async void WriteLogs(object state)
    {
        try
        {
            // Prevent overlap if method execution time exceeds timer interval.
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);

            // Drain queue, writing logs to data store.
            while (_queue?.TryDequeue(out var data) ?? false)
            {
                if (data == null) continue;
                try
                {
                    await WriteLogToDataStore(data);
                }
                catch
                {
                    // Ignore exception.  Move on to next log message.
                }
            }
        }
        finally
        {
            try
            {
                await FlushLogsToDataStore();
            }
            catch
            {
                // Ignore exception.
            }

            // Enable timer to fire again.
            _timer?.Change(_options.QueueIntervalMs, _options.QueueIntervalMs);
        }
    }


    protected abstract Task WriteLogToDataStore(LogData data);
    protected virtual async Task FlushLogsToDataStore() => await Task.CompletedTask;
}