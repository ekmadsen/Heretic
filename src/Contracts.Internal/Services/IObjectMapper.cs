namespace ErikTheCoder.Contracts.Internal.Services;


public interface IObjectMapper
{
    IAsyncEnumerable<Dtos.User> ToDtos(IAsyncEnumerable<Entities.User> users);
    Dtos.User ToDto(Entities.User user);
}