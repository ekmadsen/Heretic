namespace ErikTheCoder.Contracts.Services;


public interface IPasswordManager
{
    int Id { get; }
    string Name { get; }


    (byte[] salt, byte[] hash) Hash(string password);
    bool Validate(string password, byte[] salt, byte[] hash);
}