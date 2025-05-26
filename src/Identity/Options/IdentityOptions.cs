namespace ErikTheCoder.Identity.Options;


public record IdentityOptions
{
    public string DatabaseName { get; set; }
    public Guid KeyId { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int TokenExpirationMinutes { get; set; }
    public string PublicSigningKey { get; set; }
    public string PrivateSigningKey { get; set; }
}