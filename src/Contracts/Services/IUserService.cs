using ErikTheCoder.Contracts.Dtos;
using ErikTheCoder.Contracts.Dtos.Requests;
using ErikTheCoder.Contracts.Dtos.Responses;


namespace ErikTheCoder.Contracts.Services;


public interface IUserService
{
    AuthMetadata GetAuthMetadata();
    IAsyncEnumerable<User> GetUsers();
    Task<User> GetUser(GetUserRequest request);
    Task<LoginResponse> Login(LoginRequest request);
    Task UpdatePassword(UpdatePasswordRequest request);
}