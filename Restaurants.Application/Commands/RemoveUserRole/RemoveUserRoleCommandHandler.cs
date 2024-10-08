using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Commands.RemoveUserRole;

public class RemoveUserRoleCommandHandler(ILogger<RemoveUserRoleCommandHandler> logger,
    UserManager<User> userManager, 
    RoleManager<IdentityRole> roleManager) : IRequestHandler<RemoveUserRoleCommand>
{
    public async Task Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Removing user role : {@Request}", request);
    
        var user = await userManager.FindByEmailAsync(request.UserEmail);
        if (user == null)
        {
            throw new NotFoundException(nameof(User),request.UserEmail);
        }

        var role = await roleManager.FindByNameAsync(request.RoleName);
        if (role == null)
        {
            throw new NotFoundException(nameof(IdentityRole), request.RoleName);
        }

        await userManager.RemoveFromRoleAsync(user, role.Name!);
    }
}
