using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetDishByIdForRestaurant;

public class GetDishByIdForRestaurantQueryHandaler(IMapper mapper, 
    ILogger<GetDishByIdForRestaurantQueryHandaler> logger,
    IRestaurantsRepository repo) : IRequestHandler<GetDishByIdForRestaurantQuery, DishDto>
{
    public async Task<DishDto> Handle(GetDishByIdForRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting dish: {dishId} for the retaurant with id: {restaurantId}",request.DishId,request.RestaurantId);
    
        var restaurant = await repo.GetByIdAsync(request.RestaurantId);
        if(restaurant == null) 
        {
            throw new NotFoundException(nameof(Restaurant),request.RestaurantId.ToString());
        }

        var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId);
        if (dish == null)
        {
            throw new NotFoundException(nameof(Dish), request.DishId.ToString());
        }

        var result = mapper.Map<DishDto>(dish);

        return result;
    }
}
