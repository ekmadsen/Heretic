using Microsoft.AspNetCore.Mvc;


namespace ErikTheCoder.Contracts.Dtos.Requests;


public record GetUserRequest
{
    [FromRoute] public int Id { get; set; }
}