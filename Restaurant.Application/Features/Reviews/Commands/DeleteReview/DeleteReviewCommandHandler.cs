using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Domain.Restaurants;
using Restaurant.Domain.Results;
using Restaurant.Domain.Reviews;

namespace Restaurant.Application.Features.Reviews.Commands.DeleteReview
{
    public sealed class DeleteReviewCommandHandler(
    IReviewRepository reviewRepository,
    IRestaurantRepository restaurantRepository,
    ICacheService cacheService,
    ILogger<DeleteReviewCommandHandler> logger)
    : IRequestHandler<DeleteReviewCommand, Result<Deleted>>
    {
        public async Task<Result<Deleted>> Handle(
            DeleteReviewCommand command,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Deleting Review {ReviewId} by User {UserId}",
                command.ReviewId,
                command.UserId);

            var review = await reviewRepository.GetByIdAsync(
                command.ReviewId,
                cancellationToken);

            if (review is null)
            {
                logger.LogWarning(
                    "Review {ReviewId} not found for deletion",
                    command.ReviewId);

                return ReviewErrors.NotFound;
            }

            if (review.UserId != command.UserId)
            {
                logger.LogWarning(
                    "User {UserId} attempted to delete Review {ReviewId} owned by {OwnerId}",
                    command.UserId,
                    command.ReviewId,
                    review.UserId);

                return Error.Forbidden("Review.Forbidden", "You can only delete your own reviews.");
            }

            reviewRepository.Remove(review);
            await reviewRepository.SaveChangesAsync(cancellationToken);

            await restaurantRepository.UpdateRatingAsync(review.RestaurantId, cancellationToken);
            await restaurantRepository.SaveChangesAsync(cancellationToken);

            // Invalidate cache
            await cacheService.RemoveByTagAsync($"restaurant:{review.RestaurantId}", cancellationToken);
            await cacheService.RemoveByTagAsync($"user:{review.RestaurantId}", cancellationToken);
            await cacheService.RemoveByTagAsync("reviews", cancellationToken);

            // Remove specific review cache
            await cacheService.RemoveAsync($"review:{command.ReviewId}", cancellationToken);

            logger.LogInformation(
                "Review {ReviewId} deleted (soft) successfully by User {UserId}",
                command.ReviewId,
                command.UserId);

            return Result.Deleted;
        }
    }
}
