using ErikTheCoder.Heretic.Contracts.Dtos.Requests;
using ErikTheCoder.Heretic.Contracts.Internal.Entities;


namespace ErikTheCoder.Heretic.Contracts.Internal.Repositories;


public interface IUserRepository
{
    IAsyncEnumerable<User> GetUsers();
    Task<User> GetUser(GetUserRequest request);
}