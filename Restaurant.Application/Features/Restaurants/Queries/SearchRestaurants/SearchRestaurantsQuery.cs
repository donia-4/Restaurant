using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.Restaurants.Dtos.SearchRestaurants;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Queries.SearchRestaurants
{
    public sealed record SearchRestaurantsQuery(
    SearchRestaurantsRequest Request)
    : IRequest<Result<PaginatedList<SearchRestaurantResponse>>>, ICachedQuery
    {
        public string CacheKey =>
            $"search:restaurants:" +
            $"name={Request.Name}:" +
            $"city={Request.City}:" +
            $"cuisine={Request.CuisineType}:" +
            $"category={Request.CategoryId}:" +
            $"status={Request.Status}:" +
            $"rating={Request.MinRating}:" +
            $"page={Request.PageNumber}:" +
            $"size={Request.PageSize}";

        public string[] Tags => ["restaurants", "search"];
        public TimeSpan Expiration => TimeSpan.FromMinutes(5);
    }
}
