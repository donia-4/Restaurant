using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.Restaurants.Dtos.ChangeRestaurantAvailability;
using Restaurant.Domain.Restaurants;
using Restaurant.Domain.Restaurants.Enums;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Commands.ChangeRestaurantAvailability
{
    public sealed class ChangeRestaurantAvailabilityCommandHandler(
    IRestaurantRepository restaurantRepository, ILogger<ChangeRestaurantAvailabilityCommandHandler> logger)
    : IRequestHandler<ChangeRestaurantAvailabilityCommand, Result<ChangeRestaurantAvailabilityResponse>>
    {

        // TODO : I should validate that only the owner can change the availability and inject
        // ICurrentUserService currentUserService into the handler and check if the current user is the owner of the restaurant.
        public async Task<Result<ChangeRestaurantAvailabilityResponse>> Handle(
            ChangeRestaurantAvailabilityCommand command,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling ChangeRestaurantAvailabilityCommand for RestaurantId: {RestaurantId}", command.RestaurantId);

            var request = command.Request;

            var restaurant = await restaurantRepository.GetByIdAsync(
                command.RestaurantId,
                cancellationToken);

            if (restaurant is null)
                return RestaurantErrors.NotFound;

            //// Authorization: Only owner can change availability
            //if (restaurant.OwnerId != currentUserService.UserId)
            //    return RestaurantErrors.Unauthorized;

            Result<Updated> result = request.Status switch
            {
                RestaurantStatus.Open => restaurant.Open(),
                RestaurantStatus.Closed => restaurant.Close(),
                RestaurantStatus.TemporarilyClosed => restaurant.SetTemporarilyClosed(),
                RestaurantStatus.UnderMaintenance => restaurant.SetUnderMaintenance(),
                _ => RestaurantErrors.InvalidReviewStatus
            };

            if (result.IsError)
                return result.Errors;

            await restaurantRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Restaurant availability changed successfully for RestaurantId: {RestaurantId} to Status: {Status}", restaurant.Id, request.Status);

            return new ChangeRestaurantAvailabilityResponse(
                restaurant.Id,
                request.Status.ToString(),
                $"Restaurant is now {request.Status.ToString().ToLower()}.");
        }
    }
}
