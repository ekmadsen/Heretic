using ErikTheCoder.Heretic.Contracts.Internal.Entities;


namespace ErikTheCoder.Heretic.Contracts.Internal.Repositories;


public interface IUserRepository
{
    IAsyncEnumerable<User> GetHereticUsers();
    IAsyncEnumerable<User> GetTestUsers();
}