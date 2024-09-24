﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantQueryHandler(ILogger<RestaurantDto> logger, IRestaurantsRepository restaurantsRepository, IMapper mapper) : IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantDto>>
{
    public async Task<IEnumerable<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants");

        var restaurants = await restaurantsRepository.GetAllAsync();

        var restaurantsDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        return restaurantsDtos;
    }
}