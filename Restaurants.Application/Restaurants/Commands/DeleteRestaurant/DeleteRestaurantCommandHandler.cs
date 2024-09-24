using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommand> logger, 
        IMapper mapper, 
        IRestaurantsRepository restaurantsRepository) : IRequestHandler<DeleteRestaurantCommand, bool>
    {
        public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting restaurant number : {RestaurantId}", request.Id);

            var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
            if (restaurant == null)
                return false;

            await restaurantsRepository.Delete(restaurant);

            return true;
        }
    }
}
