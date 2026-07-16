using MediatR;
using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.Foods.Dtos.SearchFoods;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Queries.SearchFoods;

public sealed record SearchFoodsQuery(
    Guid RestaurantId,
    string? Name,
    string? CategoryName,
    decimal? MinPrice,
    decimal? MaxPrice,
    int PageNumber,
    int PageSize) : IRequest<Result<PaginatedList<SearchFoodResponse>>>, ICachedQuery
{
    public string CacheKey =>
        $"foods:search:{RestaurantId}:n={Name}:c={CategoryName}:min={MinPrice}:max={MaxPrice}:p={PageNumber}:s={PageSize}";

    public string[] Tags => ["foods", "search"];

    public TimeSpan Expiration => TimeSpan.FromMinutes(2);
}