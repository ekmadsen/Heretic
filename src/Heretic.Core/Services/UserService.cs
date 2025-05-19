using ErikTheCoder.Heretic.Contracts.Dtos;
using ErikTheCoder.Heretic.Contracts.Dtos.Requests;
using ErikTheCoder.Heretic.Contracts.Internal.Repositories;
using ErikTheCoder.Heretic.Contracts.Internal.Services;


namespace ErikTheCoder.Heretic.Core.Services;


public class UserService(IUserRepository repository, IObjectMapper objectMapper) : IUserService
{
    public IAsyncEnumerable<User> GetUsers()
    {
        var userEntities = repository.GetUsers();
        return objectMapper.ToDtos(userEntities);
    }


    public async Task<User> GetUser(GetUserRequest request)
    {
        var userEntity = await repository.GetUser(request);
        return objectMapper.ToDto(userEntity);
    }
}