using ErikTheCoder.Contracts.Dtos.Payloads;
using Microsoft.AspNetCore.Mvc;


namespace ErikTheCoder.Contracts.Dtos.Requests;


public record UpdatePasswordRequest
{
    [FromRoute] public int Id { get; set; }
    [FromBody] public UpdatePasswordPayload Payload { get; set; }
}