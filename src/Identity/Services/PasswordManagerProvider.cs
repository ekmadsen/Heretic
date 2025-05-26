using ErikTheCoder.Contracts.Services;


namespace ErikTheCoder.Identity.Services;


public class PasswordManagerProvider(IEnumerable<IPasswordManager> passwordManagers) : IPasswordManagerProvider
{
    public IPasswordManager Get(int id) => passwordManagers.FirstOrDefault(passwordManager => passwordManager.Id == id);
    public IPasswordManager Get(string name) => passwordManagers.FirstOrDefault(passwordManager => string.Equals(passwordManager.Name, name, StringComparison.OrdinalIgnoreCase));
}