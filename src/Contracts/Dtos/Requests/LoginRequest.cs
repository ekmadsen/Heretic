using ErikTheCoder.Contracts.Dtos.Payloads;
using Microsoft.AspNetCore.Mvc;


namespace ErikTheCoder.Contracts.Dtos.Requests;


public record LoginRequest
{
    [FromBody] public LoginPayload Payload { get; set; }
}