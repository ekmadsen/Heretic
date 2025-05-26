namespace ErikTheCoder.Contracts.Dtos.Payloads;


public record UpdatePasswordPayload
{
    public string Password { get; set; }
}