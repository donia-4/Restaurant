using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Features.Reviews.Dtos.GetReview;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Reviews.Queries.GetReviewById
{
    public sealed record GetReviewByIdQuery(Guid ReviewId)
    : ICachedQuery<Result<ReviewResponse>>
    {
        public string CacheKey => $"review:{ReviewId}";

        public string[] Tags => ["reviews"];

        public TimeSpan Expiration => TimeSpan.FromMinutes(5);
    }
}
