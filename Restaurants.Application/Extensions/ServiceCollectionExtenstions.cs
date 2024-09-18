using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Extensions
{
    public static class ServiceCollectionExtenstions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IRestaurantsService, RestaurantsService>();

            services.AddAutoMapper(typeof(ServiceCollectionExtenstions).Assembly);
        }
    }
}
