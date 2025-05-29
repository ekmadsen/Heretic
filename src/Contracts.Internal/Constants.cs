namespace ErikTheCoder.Contracts.Internal;


public static class PasswordManagerId
{
    public const int Pbkdf2Mod1 = 1;
}


public static class PasswordManagerName
{
    public const string Latest = Pbkdf2Mod1;
    public const string Pbkdf2Mod1 = "Pbkdf2 Mod1";
}


public static class PolicyName
{
    public const string Read = "Read";
    public const string Write = "Write";
}


public static class ClaimName
{
    public const string Scope = "scope";
}


public static class Scope
{
    public const string ApiRead = "api-read";
    public const string ApiWrite = "api-write";
}