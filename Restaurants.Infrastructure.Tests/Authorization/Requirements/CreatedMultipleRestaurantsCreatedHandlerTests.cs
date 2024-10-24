using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Infrastructure.Authorization.Requirements.Tests;

public class CreatedMultipleRestaurantsCreatedHandlerTests
{
    [Fact()]
    public async void HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {
        // Arrange

        var currentUser = new CurrentUser("1", "test@test.com", [],null,null);

        var userContext = new Mock<IUserContext>();
        userContext.Setup(m => m.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>()
        {
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = "2"
            }
        };

        var restaurantRepoMock = new Mock<IRestaurantsRepository>();
        restaurantRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantsCreated(2);
        var handler = new CreatedMultipleRestaurantsCreatedHandler(restaurantRepoMock.Object, userContext.Object);
        var context = new AuthorizationHandlerContext([requirement],null,null);

        // Act

        await handler.HandleAsync(context);

        // Assert

        context.HasSucceeded.Should().BeTrue();
    }

    [Fact()]
    public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
    {
        // Arrange

        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);

        var userContext = new Mock<IUserContext>();
        userContext.Setup(m => m.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>()
        {
            new()
            {
                OwnerId = currentUser.Id
            },           
            new()
            {
                OwnerId = "2"
            }
        };

        var restaurantRepoMock = new Mock<IRestaurantsRepository>();
        restaurantRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantsCreated(2);
        var handler = new CreatedMultipleRestaurantsCreatedHandler(restaurantRepoMock.Object, userContext.Object);
        var context = new AuthorizationHandlerContext([requirement], null, null);

        // Act

        await handler.HandleAsync(context);

        // Assert

        context.HasSucceeded.Should().BeFalse();
    }
}