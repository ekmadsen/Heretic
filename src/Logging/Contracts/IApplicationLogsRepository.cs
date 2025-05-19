using ErikTheCoder.Logging.Entities;


namespace ErikTheCoder.Logging.Contracts;


public interface IApplicationLogsRepository
{
    Task<int> InsertApplication(string name);
    Task<int> InsertComponent(string name);
    Task<int> InsertProperty(string name);
    Task<int> InsertMessage(Message message, List<PropertyValue> propertyValues = null);
}