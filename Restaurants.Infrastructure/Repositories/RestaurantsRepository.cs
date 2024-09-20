
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
{
    public async Task<int> Create(Restaurant restaurant)
    {
        dbContext.Add(restaurant);
        await dbContext.SaveChangesAsync();

        return restaurant.Id;
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var restaurants = await dbContext.Restaurants.ToListAsync();

        return restaurants;
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        var restaurant = await dbContext.Restaurants
            .Include(d => d.Dishes)
            .FirstOrDefaultAsync(x => x.Id == id);

        return restaurant;
    }    

    public async Task Delete(Restaurant restaurant)
    {
        dbContext.Restaurants.Remove(restaurant);
        await dbContext.SaveChangesAsync();        
    }

    //public async Task Update(Restaurant restaurant)
    //{
    //    dbContext.Update(restaurant);
    //    await dbContext.SaveChangesAsync();       
    //}

    public Task SaveChanges()
        => dbContext.SaveChangesAsync();
}
