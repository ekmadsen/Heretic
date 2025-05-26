using Microsoft.AspNetCore.Mvc;


namespace ErikTheCoder.Heretic.WebApi.Controllers;


[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    [HttpGet]
    [Route("foobar")]
    public async Task FooBar()
    {
        // Place test code here.

        await Task.CompletedTask;
    }
}