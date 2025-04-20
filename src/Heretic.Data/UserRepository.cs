using Dapper;
using ErikTheCoder.Data;
using ErikTheCoder.Heretic.Contracts.Internal;
using ErikTheCoder.Heretic.Contracts.Internal.Entities;
using ErikTheCoder.Heretic.Contracts.Internal.Repositories;


namespace ErikTheCoder.Heretic.Data;


public class UserRepository(IDatabaseProvider databaseProvider) : IUserRepository
{
    public IAsyncEnumerable<User> GetHereticUsers()
    {
        var database = databaseProvider.Get(DatabaseName.Heretic);
        using var connection = database.OpenConnection();

        const string sql = "select Id, Username, Email, FirstName, LastName from Users";
        return connection.QueryUnbufferedAsync<User>(sql);
    }


    public IAsyncEnumerable<User> GetTestUsers()
    {
        var database = databaseProvider.Get(DatabaseName.Test);
        var connection = database.OpenConnection();

        const string sql = "select Id, Username, Email, FirstName, LastName from Users";
        return connection.QueryUnbufferedAsync<User>(sql);
    }
}