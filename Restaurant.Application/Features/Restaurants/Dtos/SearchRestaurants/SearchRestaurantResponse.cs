using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Domain.Restaurants.Enums;

namespace Restaurant.Application.Features.Restaurants.Dtos.SearchRestaurants
{
    public sealed class SearchRestaurantResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string? Logo { get; init; }
        public CuisineType CuisineType { get; init; }
        public decimal Rating { get; init; }
        public RestaurantStatus Status { get; init; }
        public string City { get; init; } = default!;
    }
}
