using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;


namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandlerTests
{
    [Fact()]
    public async void Handle_ForValidCommand_ReturnsCreatedRestaurantId()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
        var mapperMock = new Mock<IMapper>();

        var command = new CreateRestaurantCommand();
        var restaurant = new Restaurant();

        mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);


        var restRepoMock = new Mock<IRestaurantsRepository>();
        restRepoMock
            .Setup(repo => repo.Create(It.IsAny<Restaurant>()))
            .ReturnsAsync(1);

        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser("owner_id", "test@test.com", [] , null, null);
        userContextMock.Setup(x=>x.GetCurrentUser()).Returns(currentUser);


        var commandHandler = new CreateRestaurantCommandHandler(loggerMock.Object, mapperMock.Object, restRepoMock.Object,userContextMock.Object);

        // Act

        var result = await commandHandler.Handle(command, CancellationToken.None);

        // Asset

        result.Should().Be(1);
        restaurant.OwnerId.Should().Be("owner_id");
        restRepoMock.Verify(r => r.Create(restaurant), Times.Once);

    }
}