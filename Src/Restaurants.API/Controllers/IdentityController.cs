using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Commands.AssignUserRole;
using Restaurants.Application.Commands.RemoveUserRole;
using Restaurants.Application.Commands.UpdateUserDetails;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpPatch("user")]
    public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
    {
        await mediator.Send(command);

        return NoContent();
    }

    [HttpPost("userRole")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("userRole")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> RemoveUserRole(RemoveUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}
