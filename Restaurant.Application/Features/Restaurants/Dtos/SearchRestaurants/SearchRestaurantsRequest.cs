using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Domain.Restaurants.Enums;

namespace Restaurant.Application.Features.Restaurants.Dtos.SearchRestaurants
{
    public sealed class SearchRestaurantsRequest
    {
        public string? Name { get; init; }
        public string? City { get; init; }
        public CuisineType? CuisineType { get; init; }
        public Guid? CategoryId { get; init; }
        public RestaurantStatus? Status { get; init; }
        public decimal? MinRating { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
