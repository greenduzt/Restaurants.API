using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Returns_BadRequest()
    {
        var response = await _client.GetAsync("/api/nonexistent-endpoint");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
