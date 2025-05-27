using ErikTheCoder.Contracts.Dtos.Requests;
using ErikTheCoder.Contracts.Internal.Entities;


namespace ErikTheCoder.Contracts.Internal.Repositories;


public interface IUserRepository
{
    IAsyncEnumerable<User> GetUsers();
    Task<User> GetUser(GetUserRequest request);
    Task<User> GetUser(UpdatePasswordRequest request);
    Task<User> GetUser(LoginRequest request);
    IAsyncEnumerable<Claim> GetClaims(int userId);
    Task UpdateUser(User user);
}