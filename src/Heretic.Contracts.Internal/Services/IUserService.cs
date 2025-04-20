using ErikTheCoder.Heretic.Contracts.Dtos;


namespace ErikTheCoder.Heretic.Contracts.Internal.Services;


public interface IUserService
{
    IAsyncEnumerable<User> GetHereticUsers();
    IAsyncEnumerable<User> GetTestUsers();
}