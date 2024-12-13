
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

public class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {      

       // if(dbContext.Database.GetPendingMigrations().Any())
       // {
       //     await dbContext.Database.MigrateAsync(); 
       // }

        // Check if can connect to the database

        if(await dbContext.Database.CanConnectAsync())
        {
            // Check if any data available in the database
            // If no data found
            if(!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                dbContext.Restaurants.AddRange(restaurants);
                await dbContext.SaveChangesAsync();
            }

            if(!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles =
            [
                new (UserRole.User)
                {
                    NormalizedName = UserRole.User.ToUpper(),
                },
                new (UserRole.Owner) {
                    NormalizedName = UserRole.Owner.ToUpper(),
                },
                new (UserRole.Admin) {
                    NormalizedName = UserRole.Admin.ToUpper(),
                }
            ];

        return roles;
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        User owner = new User()
        {
            Email = "seed-user@test.com"
        };

        List<Restaurant> restaurants = [
          new()
            {
                Owner = owner,
                Name = "KFC",
                Category = "Fast Food",
                Description =
                    "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
                ContactEmail = "contact@kfc.com",
                HasDelivery = true,
                Dishes =
                [
                    new ()
                    {
                        Name = "Nashville Hot Chicken",
                        Description = "Nashville Hot Chicken (10 pcs.)",
                        Price = 10.30M,
                    },

                    new ()
                    {
                        Name = "Chicken Nuggets",
                        Description = "Chicken Nuggets (5 pcs.)",
                        Price = 5.30M,
                    },
                ],
                Address = new ()
                {
                    City = "London",
                    Street = "Cork St 5",
                    PostCode = "WC2N 5DU"
                }
            },
            new ()
            {
                Owner = owner,
                Name = "McDonald",
                Category = "Fast Food",
                Description =
                    "McDonald's Corporation (McDonald's), incorporated on December 21, 1964, operates and franchises McDonald's restaurants.",
                ContactEmail = "contact@mcdonald.com",
                HasDelivery = true,
                Address = new Address()
                {
                    City = "London",
                    Street = "Boots 193",
                    PostCode = "W1F 8SR"
                }
            }
      ];
        return restaurants;
    }
}
