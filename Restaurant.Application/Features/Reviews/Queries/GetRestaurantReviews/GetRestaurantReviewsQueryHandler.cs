using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.Reviews.Dtos.GetReview;
using Restaurant.Application.Features.Reviews.Dtos.Mappings;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Reviews.Queries.GetRestaurantReviews
{
    public sealed class GetRestaurantReviewsQueryHandler(
    IReviewRepository reviewRepository,
    ILogger<GetRestaurantReviewsQueryHandler> logger)
    : IRequestHandler<GetRestaurantReviewsQuery, Result<PaginatedList<ReviewResponse>>>
    {
        public async Task<Result<PaginatedList<ReviewResponse>>> Handle(
            GetRestaurantReviewsQuery query,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Fetching reviews for Restaurant {RestaurantId} - Page {PageNumber}, Size {PageSize}",
                query.RestaurantId,
                query.PageNumber,
                query.PageSize);

            var paginatedList = await reviewRepository.GetByRestaurantAsync(
                query.RestaurantId,
                query.PageNumber,
                query.PageSize,
                cancellationToken);

            var responses = paginatedList.Items
                .Select(r => r.ToResponse())
                .ToList();

            logger.LogInformation(
                "Fetched {Count} reviews for Restaurant {RestaurantId} (Total: {TotalCount})",
                responses.Count,
                query.RestaurantId,
                paginatedList.TotalCount);

            return new PaginatedList<ReviewResponse>
            {
                PageNumber = paginatedList.PageNumber,
                PageSize = paginatedList.PageSize,
                TotalCount = paginatedList.TotalCount,
                TotalPages = paginatedList.TotalPages,
                Items = responses
            };
        }
    }
}
