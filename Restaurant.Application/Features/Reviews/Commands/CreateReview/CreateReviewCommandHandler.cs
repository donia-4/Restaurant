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

namespace Restaurant.Application.Features.Reviews.Commands.CreateReview
{
    public sealed class CreateReviewCommandHandler(
    IReviewRepository reviewRepository,
    ICacheService cacheService,
    IRestaurantRepository restaurantRepository,
    ILogger<CreateReviewCommandHandler> logger)
    : IRequestHandler<CreateReviewCommand, Result<ReviewResponse>>
    {
        public async Task<Result<ReviewResponse>> Handle(
            CreateReviewCommand command,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Creating review for Restaurant {RestaurantId} by User {UserId}",
                command.Request.RestaurantId,
                command.UserId);

            var existingReview = await reviewRepository.GetByUserAndRestaurantAsync(
                command.UserId,
                command.Request.RestaurantId,
                cancellationToken);

            if (existingReview is not null)
            {
                logger.LogWarning(
                    "User {UserId} already reviewed Restaurant {RestaurantId}",
                    command.UserId,
                    command.Request.RestaurantId);

                return ReviewErrors.AlreadyExists;
            }

            var review = Review.Create(
                Guid.NewGuid(),
                command.Request.RestaurantId,
                command.UserId,
                command.Request.Rating,
                command.Request.Comment);

            if (review.IsError)
            {
                logger.LogError(
                    "Failed to create review for Restaurant {RestaurantId}. Errors: {Errors}",
                    command.Request.RestaurantId,
                    review.Errors);

                return review.Errors;
            }

            await reviewRepository.AddAsync(review.Value, cancellationToken);
            await reviewRepository.SaveChangesAsync(cancellationToken);

            // Update restaurant rating
            await restaurantRepository.UpdateRatingAsync(command.Request.RestaurantId, cancellationToken);
            await restaurantRepository.SaveChangesAsync(cancellationToken);

            //  Invalidate cache
            await cacheService.RemoveByTagAsync($"restaurant:{command.Request.RestaurantId}", cancellationToken);
            await cacheService.RemoveByTagAsync("reviews", cancellationToken);

            logger.LogInformation(
                "Review {ReviewId} created successfully for Restaurant {RestaurantId} by User {UserId}",
                review.Value.Id,
                command.Request.RestaurantId,
                command.UserId);

            return review.Value.ToResponse();
        }
    }
}
