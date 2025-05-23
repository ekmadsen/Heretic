﻿namespace ErikTheCoder.Heretic.Contracts.Internal;


public static class DatabaseName
{
    public const string Heretic = "Heretic";
}


public static class Event
{
    // TODO: Re-number database event IDs.
    public const int PrepareCommandId = 1001;
    public const string PrepareCommandName = "PrepareCommand";
    public const int ExecuteNonQueryId = 1002;
    public const string ExecuteNonQueryName = "ExecuteNonQuery";
    public const int ExecuteScalarId = 1003;
    public const string ExecuteScalarName = "ExecuteScalar";
    public const int ExecuteDataReaderId = 1004;
    public const string ExecuteDataReaderName = "ExecuteDataReader";
    public const int OpenDatabaseId = 1005;
    public const string OpenDatabaseName = "OpenDatabase";
    public const int CommitTransactionId = 1006;
    public const string CommitTransactionName = "CommitTransaction";
    public const int RollbackTransactionId = 1007;
    public const string RollbackTransactionName = "RollbackTransaction";


    public const int FooBarId = 9001;
    public const string FooBarName = "FooBar";
    public const int HelloWorldId = 9002;
    public const string HelloWorldName = "HelloWorld";
}