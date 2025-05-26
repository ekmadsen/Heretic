namespace ErikTheCoder.Contracts.Dtos.Payloads;


public record LoginPayload
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}