using ErikTheCoder.Contracts.Dtos;
using ErikTheCoder.Contracts.Dtos.Requests;
using ErikTheCoder.Contracts.Dtos.Responses;


namespace ErikTheCoder.Contracts.Services;


public interface IUserService
{
    IAsyncEnumerable<User> GetUsers();
    Task<User> GetUser(GetUserRequest request);
    Task<LoginResponse> Login(LoginRequest request);
    Task UpdatePassword(UpdatePasswordRequest request);
}