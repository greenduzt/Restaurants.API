﻿using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.API.Tests;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Seeders;
using System.Net;
using Xunit;


namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock = new ();
    private readonly Mock<IRestaurantSeeder> _restaurantsSeederMock = new ();

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository), _ => _restaurantRepositoryMock.Object));

                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantSeeder), _ => _restaurantsSeederMock.Object));
            });
        });
    }


    [Fact()]
    public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
    {
        // Arrange

        var id = 1111;

        _restaurantRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);

        var client = _factory.CreateClient();


        // Act

        var response = await client.GetAsync($"/api/restaurants/{id}");



        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);


    }


    [Fact()]
    public async Task GetAll_ForValidRequest_Return200Ok()
    {
        // Arrange

        var client = _factory.CreateClient();

        // Act

        var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        // Assert

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact()]
    public async Task GetAll_ForInValidRequest_Return400BadRequst()
    {
        // Arrange

        var client = _factory.CreateClient();

        // Act

        var result = await client.GetAsync("/api/restaurants");

        // Assert

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact()]
    public async Task GetById_ForExistingId_ShouldReturn200Ok()
    {
        // Arrange

        var id = 99;

        var restaurant = new Restaurant()
        {
            Id = id,
            Name = "Test",
            Description = "Test Description"
        };


        _restaurantRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(restaurant);

        var client = _factory.CreateClient();


        // Act

        var response = await client.GetAsync($"/api/restaurants/{id}");
        // Read the JSON Dto
        //var restaurantDto = await response.Content.ReadAsStringAsync<RestaurantDto>();


        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);


    }
}