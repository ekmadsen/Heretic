namespace ErikTheCoder.Data;


public interface IDatabaseProvider
{
    public IDatabase Get(string name);
}