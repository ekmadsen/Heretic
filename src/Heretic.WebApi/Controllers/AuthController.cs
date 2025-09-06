using ErikTheCoder.Contracts.Dtos;
using ErikTheCoder.Contracts.Dtos.Requests;
using ErikTheCoder.Contracts.Internal;
using ErikTheCoder.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ErikTheCoder.Heretic.WebApi.Controllers;


[ApiController]
[Route("auth")]
public class AuthController(IUserService userService)
{
    [HttpGet]
    [Route("metadata")]
    [AllowAnonymous]
    public ActionResult<AuthMetadata> GetMetadata() => userService.GetAuthMetadata();


    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        var response = await userService.Login(request);
        return response.Success
            ? new OkObjectResult(response.Token)
            : new UnauthorizedResult();
    }


    [HttpPatch]
    [Route("users/{Id:int}/password")]
    [Authorize(Policy = PolicyName.Write)]
    public async Task UpdateUserPassword(UpdatePasswordRequest request)
    {
        // TODO: Verify authenticated user is an admin or is the user specified in the request DTO.
        await userService.UpdatePassword(request);
    }
}