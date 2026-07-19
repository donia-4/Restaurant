using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.Restaurants.Dtos.GetAllRestaurants;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Queries.GetAllRestaurants;

public sealed record GetAllRestaurantsQuery(
    int PageNumber = 1,
    int PageSize = 10)
    : ICachedQuery<Result<PaginatedList<GetAllRestaurantsResponse>>>
{
    public string CacheKey => $"restaurants:all:{PageNumber}:{PageSize}";

    public string[] Tags => ["restaurants"];

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}