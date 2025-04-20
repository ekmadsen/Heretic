using ErikTheCoder.Heretic.Contracts.Dtos;
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
    [Route("hereticusers")]
    public IAsyncEnumerable<User> GetHereticUsers() => userService.GetHereticUsers();


    [HttpGet]
    [Route("testusers")]
    public IAsyncEnumerable<User> GetTestUsers() => userService.GetTestUsers();
}