using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.Reviews.Dtos.GetReview;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Reviews.Queries.GetUserReviews
{
    public sealed record GetUserReviewsQuery(
    Guid UserId,
    int PageNumber = 1,
    int PageSize = 10)
    : ICachedQuery<Result<PaginatedList<ReviewResponse>>>
    {
        public string CacheKey =>
            $"reviews:user:{UserId}:page:{PageNumber}:size:{PageSize}";

        public string[] Tags => ["reviews", $"user:{UserId}"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(3);
    }
}
