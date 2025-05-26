namespace ErikTheCoder.Contracts.Services;


public interface IPasswordManagerProvider
{
    IPasswordManager Get(int id);
    IPasswordManager Get(string name);
}