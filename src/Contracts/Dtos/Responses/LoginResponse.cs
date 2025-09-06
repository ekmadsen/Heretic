namespace ErikTheCoder.Contracts.Dtos.Responses;


public record LoginResponse : ResponseBase
{
    public LoginStatus Status { get; set; }
    public string Token { get; set; }
}