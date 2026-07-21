using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.Reviews.Dtos.GetReview;
using Restaurant.Application.Features.Reviews.Dtos.Mappings;
using Restaurant.Domain.Results;
using Restaurant.Domain.Reviews;

namespace Restaurant.Application.Features.Reviews.Queries.GetReviewById
{
    public sealed class GetReviewByIdQueryHandler(
    IReviewRepository reviewRepository,
    ILogger<GetReviewByIdQueryHandler> logger)
    : IRequestHandler<GetReviewByIdQuery, Result<ReviewResponse>>
    {
        public async Task<Result<ReviewResponse>> Handle(
            GetReviewByIdQuery query,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Fetching Review {ReviewId}",
                query.ReviewId);

            var review = await reviewRepository.GetByIdWithUserAsync(
                query.ReviewId,
                cancellationToken);

            if (review is null)
            {
                logger.LogWarning(
                    "Review {ReviewId} not found",
                    query.ReviewId);

                return ReviewErrors.NotFound;
            }

            logger.LogInformation(
                "Review {ReviewId} fetched successfully",
                query.ReviewId);

            return review.ToResponse();
        }
    }

}
