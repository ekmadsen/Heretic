using ErikTheCoder.Contracts.Dtos;
using ErikTheCoder.Contracts.Dtos.Requests;
using ErikTheCoder.Contracts.Services;
using Microsoft.AspNetCore.Mvc;


namespace ErikTheCoder.Heretic.WebApi.Controllers;


[ApiController]
[Route("users")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    [Route("")]
    public IAsyncEnumerable<User> GetUsers() => userService.GetUsers();


    [HttpGet]
    [Route("{Id:int}")]
    public async Task<User> GetUser(GetUserRequest request) => await userService.GetUser(request);
}