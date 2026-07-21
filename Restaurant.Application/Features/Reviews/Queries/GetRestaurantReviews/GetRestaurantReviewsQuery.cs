using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.Reviews.Dtos.GetReview;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Reviews.Queries.GetRestaurantReviews
{
    public sealed record GetRestaurantReviewsQuery(
    Guid RestaurantId,
    int PageNumber = 1,
    int PageSize = 10)
    : ICachedQuery<Result<PaginatedList<ReviewResponse>>>
    {
        public string CacheKey =>
            $"reviews:restaurant:{RestaurantId}:page:{PageNumber}:size:{PageSize}";

        public string[] Tags => ["reviews", $"restaurant:{RestaurantId}"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(3);
    }
}
