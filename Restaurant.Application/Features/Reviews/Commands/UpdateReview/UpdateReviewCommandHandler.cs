using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Application.Features.Reviews.Dtos.GetReview;
using Restaurant.Application.Features.Reviews.Dtos.Mappings;
using Restaurant.Domain.Results;
using Restaurant.Domain.Reviews;

namespace Restaurant.Application.Features.Reviews.Commands.UpdateReview
{
    public sealed class UpdateReviewCommandHandler(
    IReviewRepository reviewRepository,
    IRestaurantRepository restaurantRepository,
    ICacheService cacheService,
    ILogger<UpdateReviewCommandHandler> logger)
    : IRequestHandler<UpdateReviewCommand, Result<ReviewResponse>>
    {
        public async Task<Result<ReviewResponse>> Handle(
            UpdateReviewCommand command,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Updating Review {ReviewId} by User {UserId}",
                command.ReviewId,
                command.UserId);

            var review = await reviewRepository.GetByIdAsync(
                command.ReviewId,
                cancellationToken);

            if (review is null)
            {
                logger.LogWarning(
                    "Review {ReviewId} not found for update",
                    command.ReviewId);

                return ReviewErrors.NotFound;
            }

            if (review.UserId != command.UserId)
            {
                logger.LogWarning(
                    "User {UserId} attempted to update Review {ReviewId} owned by {OwnerId}",
                    command.UserId,
                    command.ReviewId,
                    review.UserId);

                return Error.Forbidden("Review.Forbidden", "You can only update your own reviews.");
            }

            var updateResult = review.Update(
                command.Request.Rating,
                command.Request.Comment);

            if (updateResult.IsError)
            {
                logger.LogError(
                    "Failed to update Review {ReviewId}. Errors: {Errors}",
                    command.ReviewId,
                    updateResult.Errors);

                return updateResult.Errors;
            }

            reviewRepository.Update(review);
            await reviewRepository.SaveChangesAsync(cancellationToken);

            // Update restaurant rating
            await restaurantRepository.UpdateRatingAsync(review.RestaurantId, cancellationToken);
            await restaurantRepository.SaveChangesAsync(cancellationToken);

            // Invalidate cache
            await cacheService.RemoveByTagAsync(
                $"restaurant:{review.RestaurantId}",
                cancellationToken);
            await cacheService.RemoveByTagAsync(
                $"user:{review.UserId}",
                cancellationToken);
            await cacheService.RemoveByTagAsync(
                "reviews",
                cancellationToken);

            // Remove specific review cache
            await cacheService.RemoveAsync(
                $"review:{review.Id}",
                cancellationToken);

            logger.LogInformation(
                "Review {ReviewId} updated successfully by User {UserId}",
                command.ReviewId,
                command.UserId);

            return review.ToResponse();
        }
    }
}
