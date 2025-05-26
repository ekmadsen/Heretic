using Microsoft.IdentityModel.Tokens;


namespace ErikTheCoder.Contracts.Dtos;


public record AuthMetadata
{
    public PublicSigningKey PublicSigningKey { get; set; }
}


public record PublicSigningKey
{
    public Guid Id { get; set; }
    public string AsPkcs1Base64 { get; set; }
    public string AsPem { get; set; }
    public JsonWebKey AsJsonWebKey { get; set; }
}