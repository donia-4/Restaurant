using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.Foods.Dtos.GetRestaurantMenu;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Queries.GetRestaurantMenu;

public sealed record GetRestaurantMenuQuery(
    Guid RestaurantId,
    int PageNumber = 1,
    int PageSize = 10)
    : ICachedQuery<Result<PaginatedList<MenuCategoryResponse>>>
{
    public string CacheKey =>
        $"restaurant:{RestaurantId}:menu:{PageNumber}:{PageSize}";

    public string[] Tags =>
    [
        $"restaurant:{RestaurantId}:menu",
        "menu"
    ];

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}