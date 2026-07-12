using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Features.Restaurants.Dtos.GetAllRestaurants;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Queries.GetAllRestaurants
{
    public sealed record GetAllRestaurantsQuery
    : ICachedQuery<Result<List<GetAllRestaurantsResponse>>>
    {
        public string CacheKey => "restaurants:all";

        public string[] Tags => ["restaurants"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}
