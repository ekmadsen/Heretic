namespace ErikTheCoder.Contracts.Services;


public interface IDatabaseProvider
{
    public IDatabase Get(string name);
}