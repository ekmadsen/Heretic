using System.Collections.Concurrent;
using ErikTheCoder.Contracts.Dtos;
using ErikTheCoder.Contracts.Internal.Entities;
using ErikTheCoder.Contracts.Internal.Repositories;
using ErikTheCoder.Logging.Options;
using Microsoft.Extensions.Options;


namespace ErikTheCoder.Logging.Services;


public class DatabaseLogger(IOptions<DatabaseLoggerOptions> options, IApplicationLogsRepository repository) : ConcurrentLoggerBase(options)
{
    // SQL Server limits index keys to 1700 bytes.  The PropertyValues indices include an int (4 bytes) PropertyId column in addition to the Value column.
    // See https://learn.microsoft.com/en-us/sql/sql-server/maximum-capacity-specifications-for-sql-server?view=sql-server-ver16#-objects.
    private const int _maxShortTextLength = 1696;

    private ConcurrentDictionary<string, int> _applications = new();
    private ConcurrentDictionary<string, int> _components = new();
    private ConcurrentDictionary<string, int> _properties = new();


    ~DatabaseLogger() => DisposeInternal();


    protected override void DisposeInternal()
    {
        // Free unmanaged resources.

        // Free managed resources.
        _applications = null;
        _components = null;
        _properties = null;

        base.DisposeInternal();
    }


    protected override async ValueTask DisposeInternalAsync()
    {
        // Free unmanaged resources.

        // Free managed resources.
        _applications = null;
        _components = null;
        _properties = null;

        await base.DisposeInternalAsync();
    }


    protected override async Task WriteLogToDataStore(LogData data)
    {
        var applicationId = await GetApplicationId(data.Application ?? options.Value.Application);
        var componentId = await GetComponentId(data.Component ?? options.Value.Component);

        // Construct message entity.
        var message = new Message
        {
            Timestamp = data.Timestamp.UtcDateTime,
            LogLevel = (byte)data.LogLevel,
            ApplicationId = applicationId,
            ComponentId = componentId,
            CorrelationId = data.CorrelationId == Guid.Empty ? null : data.CorrelationId,
            Category = data.Category,
            EventId = data.EventId.Id,
            EventName = data.EventId.Name,
            Text = data.Message
        };

        // Construct list of PropertyValue entities.
        var propertyValues = new List<PropertyValue>();

        foreach (var property in data.Properties)
        {
            if (property.Value == null) continue;
            var propertyValue = new PropertyValue { PropertyId = await GetPropertyId(property.Key) };

            // Set correct Value column based on property value type.
            switch (property.Value)
            {
                case bool value:
                    propertyValue.BoolValue = value;
                    break;

                case int value:
                    propertyValue.IntValue = value;
                    break;

                case long value:
                    propertyValue.LongValue = value;
                    break;

                case decimal value:
                    propertyValue.DecimalValue = value;
                    break;

                case DateTimeOffset value:
                    propertyValue.DateTimeValue = value.UtcDateTime;
                    break;

                case DateTime value:
                    propertyValue.DateTimeValue = value;
                    break;

                default:
                    var textValue = property.Value?.ToString();
                    if (textValue?.Length > _maxShortTextLength) propertyValue.LongTextValue = textValue;
                    else propertyValue.ShortTextValue = textValue;
                    break;

            }

            propertyValues.Add(propertyValue);
        }

        // Insert message and associated property values.
        await repository.InsertMessage(message, propertyValues);
    }
    
    
    private async Task<int> GetApplicationId(string name)
    {
        if (_applications.TryGetValue(name, out var id)) return id;

        // Insert application into database and update dictionary.
        id = await repository.InsertApplication(name);
        _applications.TryAdd(name, id);
        return id;
    }


    private async Task<int> GetComponentId(string name)
    {
        if (_components.TryGetValue(name, out var id)) return id;

        // Insert component into database and update dictionary.
        id = await repository.InsertComponent(name);
        _components.TryAdd(name, id);
        return id;
    }


    private async Task<int> GetPropertyId(string name)
    {
        if (_properties.TryGetValue(name, out var id)) return id;

        // Insert property into database and update dictionary.
        id = await repository.InsertProperty(name);
        _properties.TryAdd(name, id);
        return id;
    }
}