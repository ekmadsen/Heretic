using ErikTheCoder.Heretic.Contracts.Dtos;
using ErikTheCoder.Heretic.Contracts.Dtos.Requests;
using ErikTheCoder.Heretic.Contracts.Internal.Services;
using ErikTheCoder.Heretic.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;


namespace ErikTheCoder.Heretic.WebApi.Controllers;


[ApiController]
[Route("test")]
public class TestController(ILogger<TestController> logger, IUserService userService) : ControllerBase
{
    [HttpGet]
    [Route("foobar")]
    public async Task<ActionResult<string>> FooBar()
    {
        const string foo = "foo";
        const string bar = "bar";
        const string baz = "baz";
        const string zap = "zap";

        logger.FooBar(foo, bar);
        logger.HelloWorld(baz, zap);

        return await Task.FromResult("foobar");
    }


    [HttpGet]
    [Route("users")]
    public IAsyncEnumerable<User> GetUsers() => userService.GetUsers();


    [HttpGet]
    [Route("users/{Id:int}")]
    public async Task<User> GetUser(GetUserRequest request) => await userService.GetUser(request);
}