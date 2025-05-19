namespace ErikTheCoder.Contracts.Internal.Services;


public interface IDatabaseProvider
{
    public IDatabase Get(string name);
}