using ErikTheCoder.Heretic.Contracts.Dtos;
using ErikTheCoder.Heretic.Contracts.Dtos.Requests;


namespace ErikTheCoder.Heretic.Contracts.Internal.Services;


public interface IUserService
{
    IAsyncEnumerable<User> GetUsers();
    Task<User> GetUser(GetUserRequest request);
}