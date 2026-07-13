using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Features.Branches.Dtos.GetBranchById;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Branches.Queries.GetRestaurantBranches
{
    public sealed record GetRestaurantBranchesQuery(Guid RestaurantId)
    : ICachedQuery<Result<IReadOnlyList<BranchResponse>>>
    {
        public string CacheKey => $"restaurant:{RestaurantId}:branches";

        public string[] Tags =>
        [
            $"restaurant:{RestaurantId}:branches",
        "branches"
        ];

        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}
