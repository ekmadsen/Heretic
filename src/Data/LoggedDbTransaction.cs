using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Logging;


namespace ErikTheCoder.Data;


public class LoggedDbTransaction(ILogger<DbTransaction> logger, DbTransaction transaction) : DbTransaction
{
    protected override DbConnection DbConnection => transaction.Connection;
    public override IsolationLevel IsolationLevel => transaction.IsolationLevel;


    public override void Commit()
    {
        // TODO: Log committing transaction.
        transaction.Commit();
    }


    public override void Rollback()
    {
        // TODO: Log rolling back transaction.
        transaction.Rollback();
    }
}