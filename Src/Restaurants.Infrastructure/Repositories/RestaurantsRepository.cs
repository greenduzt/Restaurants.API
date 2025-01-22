
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System.Linq.Expressions;

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

    public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, 
        int pageSize, 
        int pageNumber, 
        string? sortBy,
        SortDirection sortDirection)
    {
        var searchPharasedLower= searchPhrase?.ToLower();

        var baseQuery = dbContext.Restaurants.Where(r => searchPharasedLower == null || (r.Name.ToLower().Contains(searchPharasedLower)
                                                    || r.Description.ToLower().Contains(searchPharasedLower)));

        var totalCount =await baseQuery.CountAsync();

        if (sortBy != null)
        {
            var columnSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>()
            {
                { nameof(Restaurant.Name), r=> r.Name },
                { nameof(Restaurant.Description), r=> r.Description },
                { nameof(Restaurant.Category), r=> r.Category }
            };

            var selectedColumn = columnSelector[sortBy];

            baseQuery = sortDirection== SortDirection.Ascending ? baseQuery.OrderBy(selectedColumn) : baseQuery.OrderByDescending(selectedColumn);
               
        }

        var restaurants = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (restaurants, totalCount);
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        var restaurant = await dbContext.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(x => x.Id == id);

        return restaurant;
    }

    public async Task Delete(Restaurant restaurant)
    {
        dbContext.Restaurants.Remove(restaurant);
        await dbContext.SaveChangesAsync();        
    }
 

    public Task SaveChanges()
        => dbContext.SaveChangesAsync();
}
