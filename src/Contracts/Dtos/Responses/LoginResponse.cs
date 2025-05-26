namespace ErikTheCoder.Contracts.Dtos.Responses;


public record LoginResponse
{
    public bool Success { get; set; }
    public LoginStatus Status { get; set; }
    public string Token { get; set; }
}