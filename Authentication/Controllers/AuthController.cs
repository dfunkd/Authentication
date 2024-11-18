using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : Controller
{
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest model, CancellationToken cancellationToken = default)
    {
        var response = await authService.Authenticate(model, cancellationToken);

        return response == null ? BadRequest(new { message = "Username or password is incorrect" }) : Ok(response);
    }
}
