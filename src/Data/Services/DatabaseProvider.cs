using ErikTheCoder.Contracts.Internal.Services;


namespace ErikTheCoder.Data.Services;


public class DatabaseProvider : IDatabaseProvider
{
    private readonly Dictionary<string, IDatabase> _databases;


    public DatabaseProvider(IEnumerable<IDatabase> databases)
    {
        _databases = new Dictionary<string, IDatabase>(StringComparer.OrdinalIgnoreCase);
        foreach (var database in databases)
            _databases.Add(database.Name, database);
    }


    public IDatabase Get(string name) => _databases.GetValueOrDefault(name);
}