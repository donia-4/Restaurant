using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.Categories.Dtos.GetRestaurantCategories;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Categories.Queries.GetRestaurantCategories;

public sealed record GetRestaurantCategoriesQuery(
    Guid RestaurantId,
    int PageNumber = 1,
    int PageSize = 10)
    : ICachedQuery<Result<PaginatedList<CategoryResponse>>>
{
    public string CacheKey => $"restaurant:{RestaurantId}:categories:{PageNumber}:{PageSize}";

    public string[] Tags =>
    [
        $"restaurant:{RestaurantId}:categories",
        "categories"
    ];

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}