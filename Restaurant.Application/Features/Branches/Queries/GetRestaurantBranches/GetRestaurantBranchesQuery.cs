using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.Branches.Dtos.GetBranchById;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Branches.Queries.GetRestaurantBranches;

public sealed record GetRestaurantBranchesQuery(
    Guid RestaurantId,
    int PageNumber = 1,
    int PageSize = 10)
    : ICachedQuery<Result<PaginatedList<BranchResponse>>>
{
    public string CacheKey =>
        $"restaurant:{RestaurantId}:branches:{PageNumber}:{PageSize}";

    public string[] Tags =>
    [
        $"restaurant:{RestaurantId}:branches",
        "branches"
    ];

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}