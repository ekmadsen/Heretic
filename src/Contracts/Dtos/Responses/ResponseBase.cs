namespace ErikTheCoder.Contracts.Dtos.Responses;


public abstract record ResponseBase
{
    public bool Success { get; set; }
    public List<string> Messages { get; set; } = [];
}