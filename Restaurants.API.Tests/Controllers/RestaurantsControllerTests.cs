using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;


namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
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
}