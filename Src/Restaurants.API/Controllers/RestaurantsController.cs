﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
[Authorize]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    //[Authorize(Policy = PolicyNames.CreatedAtLeast2Restaurants)]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll([FromQuery] GetAllRestaurantsQuery query)
    {
        var restaulrants = await mediator.Send(query);

        return Ok(restaulrants);
    }

    [HttpGet("{id}")]
    //[Authorize(Policy = PolicyNames.HasNationality)]
    public async Task<ActionResult<RestaurantDto>> GettById([FromRoute]int id)
    {
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
               
        return Ok(restaurant);    
    }
    
    [HttpPost]
    [Authorize(Roles = UserRole.Owner)]
    public async Task<IActionResult> CreateRestaurant(CreateRestaurantCommand createRestaurantCommand)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        int id = await mediator.Send(createRestaurantCommand);

        return CreatedAtAction(nameof(GettById),new { id },null);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
    {
       await mediator.Send(new DeleteRestaurantCommand(id));
               
       return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRestaurant([FromRoute]int id,[FromBody] UpdateRestaurantCommand updateRestaurantCommand)
    {
        updateRestaurantCommand.Id = id;
        await mediator.Send(updateRestaurantCommand);
        
        return NoContent();        
    }


    [HttpPost("{id}/logo")]
    public async Task<IActionResult> UploadLogo([FromRoute] int id, IFormFile file)
    {
        using var stream = file.OpenReadStream();

        var command = new UploadRestaurantLogoCommand()
        {
            RestaurantId = id,
            FileName = $"{id}-{file.FileName}",
            File = stream
        };

        await mediator.Send(command);
        return NoContent();
    }
}
