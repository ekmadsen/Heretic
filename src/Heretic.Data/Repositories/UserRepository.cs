using Dapper;
using ErikTheCoder.Data;
using ErikTheCoder.Heretic.Contracts.Dtos.Requests;
using ErikTheCoder.Heretic.Contracts.Internal;
using ErikTheCoder.Heretic.Contracts.Internal.Entities;
using ErikTheCoder.Heretic.Contracts.Internal.Repositories;


namespace ErikTheCoder.Heretic.Data.Repositories;


public class UserRepository(IDatabaseProvider databaseProvider) : IUserRepository
{
    public IAsyncEnumerable<User> GetUsers()
    {
        var database = databaseProvider.Get(DatabaseName.Heretic);
        using var connection = database.OpenConnection();

        const string sql = "select Id, Username, Email, FirstName, LastName from Users";
        return connection.QueryUnbufferedAsync<User>(sql);
    }


    public async Task<User> GetUser(GetUserRequest request)
    {
        var database = databaseProvider.Get(DatabaseName.Heretic);
        await using var connection = await database.OpenConnectionAsync();

        const string sql = "select * from Users where Id = @id";
        return await connection.QueryFirstAsync<User>(sql, request);
    }
}