using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using System.Reflection.Metadata;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantsRepository> _restRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restAuthServiceMock;

    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _restRepoMock = new Mock<IRestaurantsRepository>();
        _mapperMock = new Mock<IMapper>();
        _restAuthServiceMock = new Mock<IRestaurantAuthorizationService>();

        _handler = new UpdateRestaurantCommandHandler(_loggerMock.Object,_restRepoMock.Object,_mapperMock.Object,_restAuthServiceMock.Object);
}

    [Fact]
    public async void Handle_WithValidRequest_ShouldUpdateRestaurant()
    {
        // Arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand()
        { 
            Id = restaurantId,
            Name = "New Test",
            Description = "New Description",
            HasDelivery = true
        
        };
                
        var restaurant = new Restaurant() 
        {
            Id = restaurantId,
            Name = "Test",
            Description = "Test"
        };

        _restRepoMock.Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync(restaurant);

        _restAuthServiceMock.Setup(m => m.Authorize(restaurant,Domain.Constants.ResourceOperation.Update)).Returns(true);

        // Act

        await _handler.Handle(command, CancellationToken.None);

        // Asset
        _restRepoMock.Verify(r => r.SaveChanges(), Times.Once);
        _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);
    
    }


    [Fact]
    public async Task Handle_WithNonExistingRestaurant_ShouldThrowNotFoundException()
    {
        // Arrange
        var restaurantId = 2;
        var request = new UpdateRestaurantCommand()
        {
            Id = restaurantId
        };

        _restRepoMock.Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync((Restaurant?)null);

        // Act

        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Restaurant with id: {restaurantId} doesn't exist");
    }

    [Fact]
    public async Task Handle_WithUnAuthorizedUser_ShouldThrowForbidException()
    {
        // Arrange
        var restaurantId = 3;
        var request = new UpdateRestaurantCommand()
        {
            Id = restaurantId
        };

        var existingRestaurant = new Restaurant()
        {
            Id=restaurantId
        };

        _restRepoMock.Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync(existingRestaurant);

        _restAuthServiceMock.Setup(a => a.Authorize(existingRestaurant, ResourceOperation.Update))
            .Returns(false);

        // Act

        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbidException>();
    }
 }
