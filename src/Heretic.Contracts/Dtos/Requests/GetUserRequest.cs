using Microsoft.AspNetCore.Mvc;


namespace ErikTheCoder.Heretic.Contracts.Dtos.Requests;


public record GetUserRequest
{
    [FromRoute]
    public int Id { get; set; }
}