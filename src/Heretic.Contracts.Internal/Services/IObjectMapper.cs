namespace ErikTheCoder.Heretic.Contracts.Internal.Services;


public interface IObjectMapper
{
    IAsyncEnumerable<Contracts.Dtos.User> ToDtos(IAsyncEnumerable<Entities.User> users);
    Contracts.Dtos.User ToDto(Entities.User user);
}