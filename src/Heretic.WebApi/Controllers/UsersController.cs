using ErikTheCoder.Contracts.Dtos;
using ErikTheCoder.Contracts.Dtos.Requests;
using ErikTheCoder.Contracts.Internal;
using ErikTheCoder.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ErikTheCoder.Heretic.WebApi.Controllers;


[ApiController]
[Route("users")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    [Route("")]
    [Authorize(Policy = PolicyName.Read)]
    public IAsyncEnumerable<User> GetUsers() => userService.GetUsers();


    [HttpGet]
    [Route("{Id:int}")]
    [Authorize(Policy = PolicyName.Read)]
    public async Task<User> GetUser(GetUserRequest request) => await userService.GetUser(request);
}