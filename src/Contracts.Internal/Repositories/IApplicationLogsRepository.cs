using ErikTheCoder.Contracts.Internal.Entities;


namespace ErikTheCoder.Contracts.Internal.Repositories;


public interface IApplicationLogsRepository
{
    Task<int> InsertApplication(string name);
    Task<int> InsertComponent(string name);
    Task<int> InsertProperty(string name);
    Task<int> InsertMessage(Message message, List<PropertyValue> propertyValues = null);
}