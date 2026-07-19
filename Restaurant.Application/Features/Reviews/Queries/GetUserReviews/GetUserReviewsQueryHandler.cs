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

namespace Restaurant.Application.Features.Reviews.Queries.GetUserReviews
{
    public sealed class GetUserReviewsQueryHandler(
    IReviewRepository reviewRepository,
    ILogger<GetUserReviewsQueryHandler> logger)
    : IRequestHandler<GetUserReviewsQuery, Result<PaginatedList<ReviewResponse>>>
    {
        public async Task<Result<PaginatedList<ReviewResponse>>> Handle(
            GetUserReviewsQuery query,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Fetching reviews for User {UserId} - Page {PageNumber}, Size {PageSize}",
                query.UserId,
                query.PageNumber,
                query.PageSize);

            var paginatedList = await reviewRepository.GetByUserAsync(
                query.UserId,
                query.PageNumber,
                query.PageSize,
                cancellationToken);

            var responses = paginatedList.Items
                .Select(r => r.ToResponse())
                .ToList();

            logger.LogInformation(
                "Fetched {Count} reviews for User {UserId} (Total: {TotalCount})",
                responses.Count,
                query.UserId,
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
