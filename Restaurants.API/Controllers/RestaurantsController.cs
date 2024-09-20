using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaulrants = await mediator.Send(new GetAllRestaurantsQuery());

        return Ok(restaulrants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GettById([FromRoute]int id)
    {
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));

        if (restaurant == null)
        {
            return NotFound("Restaurant not found");
        }

        return Ok(restaurant);
    
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand createRestaurantCommand)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        int id = await mediator.Send(createRestaurantCommand);

        return CreatedAtAction(nameof(GettById),new { id },null);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
    {
        var isDeleted = await mediator.Send(new DeleteRestaurantCommand(id));

        if (isDeleted)
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateRestaurant([FromRoute]int id,[FromBody] UpdateRestaurantCommand updateRestaurantCommand)
    {
        updateRestaurantCommand.Id = id;
        bool isUpdated = await mediator.Send(updateRestaurantCommand);
        if(isUpdated) 
            return NoContent();

        return NotFound();
    }
}
