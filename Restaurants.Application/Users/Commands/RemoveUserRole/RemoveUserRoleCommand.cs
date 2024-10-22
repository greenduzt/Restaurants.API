using MediatR;

namespace Restaurants.Application.Commands.RemoveUserRole;

public class RemoveUserRoleCommand : IRequest
{
    public string UserEmail { get; set; } = default;
    public string RoleName { get; set; } = default;

}
