
using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class CreatedMultipleRestaurantsCreatedHandler(IRestaurantsRepository restaurantsRepository,
    IUserContext userContext) : AuthorizationHandler<CreatedMultipleRestaurantsCreated>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsCreated requirement)
    {
        var currentUser = userContext.GetCurrentUser();

        var restaurants = await restaurantsRepository.GetAllAsync();

        var userRestaurantsCreated = restaurants.Count(x => x.OwnerId == currentUser!.Id);


        if (userRestaurantsCreated >= requirement.MinimumRestaurantsCreated) 
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
