using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.IntegrationEvents;
using Restaurant.Application.Common.Interfaces.Messaging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Messages;
using Restaurant.Application.Features.Restaurants.Dtos.ReviewRestaurant;
using Restaurant.Domain.Restaurants;
using Restaurant.Domain.Restaurants.Enums;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Commands.ReviewRestaurant
{
    public sealed class ReviewRestaurantCommandHandler(
        IRestaurantRepository restaurantRepository,
        IEventPublisher eventPublisher,
        ILogger<ReviewRestaurantCommandHandler> logger)
        : IRequestHandler<ReviewRestaurantCommand, Result<ReviewRestaurantResponse>>
    {
        public async Task<Result<ReviewRestaurantResponse>> Handle(
            ReviewRestaurantCommand command,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Processing ReviewRestaurantCommand for Restaurant ID: {RestaurantId}",
                command.RestaurantId);

            var request = command.Request;

            var restaurant = await restaurantRepository.GetByIdAsync(
                command.RestaurantId,
                cancellationToken);

            if (restaurant is null)
                return RestaurantErrors.NotFound;

            // Capture previous status before change
            var previousStatus = restaurant.Status.ToString();

            Result<Updated> result = request.Status switch
            {
                RestaurantStatus.Approved => restaurant.Approve(),
                RestaurantStatus.Rejected => restaurant.Reject(request.Reason),
                RestaurantStatus.Pending => restaurant.RequestModification(),
                _ => RestaurantErrors.InvalidReviewStatus
            };

            if (result.IsError)
                return result.Errors;

            await restaurantRepository.SaveChangesAsync(cancellationToken);

            var currentStatus = restaurant.Status.ToString();

            // Publish Integration Events based on status
            if (request.Status == RestaurantStatus.Approved)
            {
                await eventPublisher.PublishAsync(
                    new RestaurantApprovedIntegrationEvent(
                        restaurant.Id,
                        restaurant.OwnerId,
                        restaurant.Name,
                        DateTime.UtcNow),
                    RoutingKeys.RestaurantApproved,
                    cancellationToken);
            }
            else if (request.Status == RestaurantStatus.Rejected)
            {
                await eventPublisher.PublishAsync(
                    new RestaurantRejectedIntegrationEvent(
                        restaurant.Id,
                        restaurant.OwnerId,
                        restaurant.Name,
                        request.Reason ?? string.Empty,
                        DateTime.UtcNow),
                    RoutingKeys.RestaurantRejected,
                    cancellationToken);
            }

            // Always publish generic status changed event
            if (previousStatus != currentStatus)
            {
                await eventPublisher.PublishAsync(
                    new RestaurantStatusChangedIntegrationEvent(
                        restaurant.Id,
                        previousStatus,
                        currentStatus,
                        DateTime.UtcNow),
                    RoutingKeys.RestaurantStatusChanged,
                    cancellationToken);
            }

            logger.LogInformation(
                "Restaurant ID: {RestaurantId} has been {Status}",
                restaurant.Id,
                request.Status.ToString().ToLower());

            return new ReviewRestaurantResponse(
                restaurant.Id,
                request.Status.ToString(),
                $"Restaurant has been {request.Status.ToString().ToLower()}.");
        }
    }
}