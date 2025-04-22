using System.Data;
using System.Data.Common;
using ErikTheCoder.Data.Extensions;
using Microsoft.Extensions.Logging;


namespace ErikTheCoder.Data;


public class LoggedDbTransaction(ILogger<DbTransaction> logger, DbTransaction transaction) : DbTransaction
{
    protected override DbConnection DbConnection => transaction.Connection;
    public override IsolationLevel IsolationLevel => transaction.IsolationLevel;


    public override void Commit()
    {
        logger.CommitTransaction();
        transaction.Commit();
    }


    public override void Rollback()
    {
        logger.RollbackTransaction();
        transaction.Rollback();
    }
}