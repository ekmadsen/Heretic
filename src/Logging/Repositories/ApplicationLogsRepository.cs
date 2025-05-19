using System.Data;
using Dapper;
using ErikTheCoder.Contracts.Internal.Entities;
using ErikTheCoder.Contracts.Internal.Repositories;
using ErikTheCoder.Logging.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;


namespace ErikTheCoder.Logging.Repositories;


public class ApplicationLogsRepository(IOptions<DatabaseLoggerOptions> options) : IApplicationLogsRepository
{
    public async Task<int> InsertApplication(string name)
    {
        await using var connection = new SqlConnection(options.Value.Connection);
        await connection.OpenAsync();

        const string idParameter = "@id";
        var parameters = new DynamicParameters();
        parameters.Add("@name", name);
        parameters.Add(idParameter, dbType: DbType.Int32, direction: ParameterDirection.Output );

        await connection.ExecuteAsync("InsertApplication", parameters, commandType: CommandType.StoredProcedure);
        return parameters.Get<int>(idParameter);
    }


    public async Task<int> InsertComponent(string name)
    {
        await using var connection = new SqlConnection(options.Value.Connection);
        await connection.OpenAsync();

        const string idParameter = "@id";
        var parameters = new DynamicParameters();
        parameters.Add("@name", name);
        parameters.Add(idParameter, dbType: DbType.Int32, direction: ParameterDirection.Output);

        await connection.ExecuteAsync("InsertComponent", parameters, commandType: CommandType.StoredProcedure);
        return parameters.Get<int>(idParameter);
    }


    public async Task<int> InsertProperty(string name)
    {
        await using var connection = new SqlConnection(options.Value.Connection);
        await connection.OpenAsync();

        const string idParameter = "@id";
        var parameters = new DynamicParameters();
        parameters.Add("@name", name);
        parameters.Add(idParameter, dbType: DbType.Int32, direction: ParameterDirection.Output);

        await connection.ExecuteAsync("InsertProperty", parameters, commandType: CommandType.StoredProcedure);
        return parameters.Get<int>(idParameter);
    }


    public async Task<int> InsertMessage(Message message, List<PropertyValue> propertyValues)
    {
        propertyValues ??= [];
        int messageId;

        // Use a transaction to atomically insert message and associated transactions, or neither.
        await using var connection = new SqlConnection(options.Value.Connection);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            // Insert message.
            const string insertMessageSql = """
                insert into Messages(Timestamp, LogLevel, ApplicationId, ComponentId, CorrelationId, Category, EventId, EventName, Text)
                output inserted.id
                values (@timestamp, @logLevel, @applicationId, @componentId, @correlationId, @category, @eventId, @eventName, @text)
                """;

            messageId = (int)(await connection.ExecuteScalarAsync(insertMessageSql, message, transaction) ?? 0);

            if (propertyValues.Count > 0)
            {
                // Set message ID on property values.
                foreach (var propertyValue in propertyValues)
                    propertyValue.MessageId = messageId;

                // Insert property values.
                const string insertPropertyValuesSql = """
                    insert into PropertyValues(MessageId, PropertyId, BoolValue, IntValue, LongValue, DecimalValue, DateTimeValue, ShortTextValue, LongTextValue)
                    values (@messageId, @propertyId, @boolValue, @intValue, @longValue, @decimalValue, @dateTimeValue, @shortTextValue, @longTextValue)
                """;
                await connection.ExecuteAsync(insertPropertyValuesSql, propertyValues, transaction);
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return messageId;
    }
}