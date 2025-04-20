using System.Data.Common;


namespace ErikTheCoder.Data;


public interface IDatabase
{
    string Name { get; }


    DbConnection OpenConnection();
    Task<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default);
}