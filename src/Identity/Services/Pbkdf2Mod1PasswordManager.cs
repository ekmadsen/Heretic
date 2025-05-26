using System.Security.Cryptography;
using ErikTheCoder.Contracts.Internal;
using ErikTheCoder.Contracts.Services;


namespace ErikTheCoder.Identity.Services;


public class Pbkdf2Mod1PasswordManager(IThreadsafeRandom random) : IPasswordManager
{
    private const int _saltBytesLength = 16;
    private const int _iterations = 36_333;
    private const int _hashBytesLength = 32;
    private readonly HashAlgorithmName _pseudoRandomFunction = HashAlgorithmName.SHA512;


    public int Id => PasswordManagerId.Pbkdf2Mod1;
    public string Name => PasswordManagerName.Pbkdf2Mod1;


    public (byte[] salt, byte[] hash) Hash(string password)
    {
        // Create random salt.
        // Storing a salt with a hashed password prevents identical passwords from hashing to the same stored value.
        // See https://security.stackexchange.com/questions/17421/how-to-store-salt
        var salt = new byte[_saltBytesLength];
        random.NextBytes(salt);

        // Get derived bytes from the combined salt and password, using the specified number of iterations.
        var hash = new byte[_hashBytesLength];
        Rfc2898DeriveBytes.Pbkdf2(password, salt, hash, _iterations, _pseudoRandomFunction);
        return (salt, hash);
    }

    
    public bool Validate(string password, byte[] salt, byte[] hash)
    {
        // Get derived bytes from the combined salt and password, using the specified number of iterations.
        var computedHash = new byte[_hashBytesLength];
        Rfc2898DeriveBytes.Pbkdf2(password, salt, computedHash, _iterations, _pseudoRandomFunction);

        return computedHash.SequenceEqual(hash);
    }
}