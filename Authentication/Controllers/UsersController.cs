using Authentication.Core.Helpers;
using Authentication.Models;
using Authentication.Models.Requests;
using Authentication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : Controller
{
    [Authorize]
    [HttpPost("AddUser")]
    public async Task<User?> AddUser([FromBody] AddUserRequest user, CancellationToken cancellationToken = default)
        => await userService.AddUser(user);

    [Authorize]
    [HttpDelete("DeleteUser")]
    public async Task<bool> DeleteUser(string userId, CancellationToken cancellationToken = default)
    {
        bool success = false;
        if (Guid.TryParse(userId, out Guid id))
            success = await userService.DeleteUser(id, cancellationToken);

        return success;
    }

    [Authorize]
    [HttpGet("GetAllUsers")]
    public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default)
        => await userService.GetAllUsers(cancellationToken);

    [Authorize]
    [HttpGet("GetUserById")]
    public async Task<User?> GetUserById(Guid userId, CancellationToken cancellationToken = default)
        => await userService.GetUserById(userId, cancellationToken);

    [Authorize]
    [HttpPut("UpdateUser")]
    public async Task<User?> UpdateUser([FromBody] UpdateUserRequest user, CancellationToken cancellationToken = default)
        => await userService.UpdateUser(user, cancellationToken);
}
