namespace ErikTheCoder.Heretic.WebApi.Options;


public record AuthOptions
{
    public Guid KeyId { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string PublicSigningKey { get; set; }
}
