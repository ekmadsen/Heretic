using Microsoft.AspNetCore.Mvc;


namespace ErikTheCoder.Heretic.WebApi.Controllers;


[ApiController]
[Route("test")]
public class TestController(ILogger<TestController> logger) : ControllerBase
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
}