using System.Data.Common;


namespace ErikTheCoder.Contracts.Internal.Services;


public interface IDatabase
{
    string Name { get; }


    DbConnection OpenConnection();
    Task<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default);
}