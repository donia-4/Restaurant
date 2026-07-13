using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Features.Branches.Dtos.GetBranchById;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Branches.Queries.GetBranchById
{
    public sealed record GetBranchByIdQuery(Guid BranchId)
    : ICachedQuery<Result<BranchResponse>>
    {
        public string CacheKey => $"branch:{BranchId}";

        public string[] Tags =>
        [
            $"branch:{BranchId}",
        "branches"
        ];

        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}
