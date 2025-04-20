using ErikTheCoder.Heretic.Contracts.Internal.Repositories;
using ErikTheCoder.Heretic.Contracts.Internal.Services;
using ErikTheCoder.ObjectMapper;
using Entities = ErikTheCoder.Heretic.Contracts.Internal.Entities;
using Dtos = ErikTheCoder.Heretic.Contracts.Dtos;


namespace ErikTheCoder.Heretic.Core.Services;


public class UserService(IUserRepository repository, IObjectMapper objectMapper) : IUserService
{
    public IAsyncEnumerable<Dtos.User> GetHereticUsers()
    {
        var hereticEntities = repository.GetHereticUsers();
        return objectMapper.Map<Entities.User, Dtos.User>(hereticEntities);
    }


    public IAsyncEnumerable<Dtos.User> GetTestUsers()
    {
        var hereticEntities = repository.GetTestUsers();
        return objectMapper.Map<Entities.User, Dtos.User>(hereticEntities);
    }
}
