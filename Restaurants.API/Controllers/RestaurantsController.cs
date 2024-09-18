using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(IRestaurantsService restaurantsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaulrants = await restaurantsService.GetAllRestaurants();

        return Ok(restaulrants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GettById([FromRoute]int id)
    {
        var restaurant = await restaurantsService.GetRestaurantById(id);

        if (restaurant == null)
        {
            return NotFound("Restaurant not found");
        }

        return Ok(restaurant);
    
    }
}
