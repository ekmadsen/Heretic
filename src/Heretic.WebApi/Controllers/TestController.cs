using ErikTheCoder.Contracts.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ErikTheCoder.Heretic.WebApi.Controllers;


[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    [HttpGet]
    [Route("foobar")]
    [Authorize(Policy = PolicyName.Read)]
    public async Task FooBar()
    {
        // Place test code here.

        await Task.CompletedTask;
    }
}