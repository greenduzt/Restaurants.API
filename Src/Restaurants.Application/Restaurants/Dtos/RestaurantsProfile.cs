﻿using AutoMapper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        CreateMap<CreateRestaurantCommand, Restaurant>()
           .ForMember(d => d.Address, opt => opt.MapFrom(
               src => new Address
               {
                   City = src.City,
                   PostCode = src.PostCode,
                   Street = src.Street
               }));

        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(d => d.City, opt =>
                opt.MapFrom(src => src.Address == null ? null : src.Address.City))
            .ForMember(d => d.PostCode, opt =>
                opt.MapFrom(src => src.Address == null ? null : src.Address.PostCode))
            .ForMember(d => d.Street, opt =>
                opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
            .ForMember(d => d.Dishes, opt => opt.MapFrom(src => src.Dishes));


        CreateMap<UpdateRestaurantCommand, Restaurant>()
             .ForMember(d => d.Address, opt => opt.MapFrom(
               src => new Address
               {
                   City = src.City,
                   PostCode = src.PostCode,
                   Street = src.Street
               }));
    }
}
