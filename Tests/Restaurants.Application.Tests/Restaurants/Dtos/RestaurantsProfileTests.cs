using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Dtos;

public class RestaurantsProfileTests
{

    private IMapper _mapper;

    public RestaurantsProfileTests()
    {
        var configuration = new MapperConfiguration(config => config.AddProfile<RestaurantsProfile>());


        _mapper = configuration.CreateMapper();
    }
    [Fact()]
    public void CreateMap_ForRestaurantToRestaurantDto_MapCorrectly()
    {
        // Arrange      


        var restaurant = new Restaurant()
        {
             Id = 1,
             Name = "Test Restaurant",
             Description = "Test Description",
             Category = "Sri Lankan",
             HasDelivery = true,
             ContactEmail = "oz@gmail.com",
             ContactNumber = "0022656",
             Address = new Address()
             {
                 City = "Test City",
                 Street = "Test Street",
                 PostCode = "43-678"
             }
        };

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        // Assert

        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);       
        restaurantDto.City.Should().Be(restaurant.Address.City);
        restaurantDto.Street.Should().Be(restaurant.Address.Street);
        restaurantDto.PostCode.Should().Be(restaurant.Address.PostCode);

    }


    [Fact()]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapCorrectly()
    {
        // Arrange     


        var command = new CreateRestaurantCommand()
        {           
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Sri Lankan",
            HasDelivery = true,
            ContactEmail = "oz@gmail.com",
            ContactNumber = "0022656",
            City = "Test City",
            Street = "Test Street",
            PostCode = "12345"            
        };

        var restaurant = _mapper.Map<Restaurant>(command);

        // Assert

        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.Category.Should().Be(command.Category);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        restaurant.ContactEmail.Should().Be(command.ContactEmail);
        restaurant.ContactNumber.Should().Be(command.ContactNumber);
        restaurant.Address.Should().NotBeNull();
        restaurant.Address.City.Should().Be(command.City);
        restaurant.Address.Street.Should().Be(command.Street);
        restaurant.Address.PostCode.Should().Be(command.PostCode);

    }

    [Fact()]
    public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapCorrectly()
    {
        // Arrange     


        var command = new UpdateRestaurantCommand()
        {
            Name = "Update Restaurant",
            Description = "Update Description",
            Category = "Sri Lankan",
            HasDelivery = false          
        };

        var restaurant = _mapper.Map<Restaurant>(command);

        // Assert

        restaurant.Should().NotBeNull();
        restaurant.Id.Should().Be(command.Id);
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);      

    }
}