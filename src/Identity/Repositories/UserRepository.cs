using Dapper;
using ErikTheCoder.Contracts.Dtos.Requests;
using ErikTheCoder.Contracts.Internal.Entities;
using ErikTheCoder.Contracts.Internal.Repositories;
using ErikTheCoder.Contracts.Services;
using ErikTheCoder.Identity.Options;
using Microsoft.Extensions.Options;


namespace ErikTheCoder.Identity.Repositories;


public class UserRepository(IOptions<IdentityOptions> options, IDatabaseProvider databaseProvider) : IUserRepository
{
    public IAsyncEnumerable<User> GetUsers()
    {
        var database = databaseProvider.Get(options.Value.DatabaseName);
        using var connection = database.OpenConnection();

        const string sql = "select Id, Username, Email, FirstName, LastName, Salt, PasswordHash, PasswordManagerId, Created, LastLogin, PasswordChanged, Active from Users where Active = 1";
        return connection.QueryUnbufferedAsync<User>(sql);
    }


    public async Task<User> GetUser(GetUserRequest request)
    {
        var database = databaseProvider.Get(options.Value.DatabaseName);
        await using var connection = await database.OpenConnectionAsync();

        const string sql = "select Id, Username, Email, FirstName, LastName, Salt, PasswordHash, PasswordManagerId, Created, LastLogin, PasswordChanged, Active from Users where Active = 1 and Id = @id";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, request);
    }


    public async Task<User> GetUser(UpdatePasswordRequest request) => await GetUser(new GetUserRequest { Id = request.Id });


    public async Task<User> GetUser(LoginRequest request)
    {
        var database = databaseProvider.Get(options.Value.DatabaseName);
        await using var connection = await database.OpenConnectionAsync();

        // Attempt to get user by username.
        const string usernameSql = "select Id, Username, Email, FirstName, LastName, Salt, PasswordHash, PasswordManagerId, Created, LastLogin, PasswordChanged, Active from Users where Active = 1 and Username = @username";
        var user = await connection.QueryFirstOrDefaultAsync<User>(usernameSql, request.Payload);
        if (user != null) return user;

        // Attempt to get user by email.
        const string emailSql = "select Id, Username, Email, FirstName, LastName, Salt, PasswordHash, PasswordManagerId, Created, LastLogin, PasswordChanged, Active from Users where Active = 1 and Email = @email";
        return await connection.QueryFirstOrDefaultAsync<User>(emailSql, request.Payload);
    }


    public async Task UpdateUser(User user)
    {
        var database = databaseProvider.Get(options.Value.DatabaseName);
        await using var connection = await database.OpenConnectionAsync();

        const string sql = """
            update Users
            set Username = @username, Email = @email, FirstName = @firstName, LastName = @lastName,
                Salt = @salt, PasswordHash = @passwordHash, PasswordManagerId = @passwordManagerId,
                Created = @created, LastLogin = @lastLogin, PasswordChanged = @passwordChanged, Active = @active
            where Id = @id
        """;

        await connection.ExecuteAsync(sql, user);
    }
}