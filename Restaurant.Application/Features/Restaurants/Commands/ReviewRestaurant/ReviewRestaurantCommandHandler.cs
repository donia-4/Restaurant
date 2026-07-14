using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.Restaurants.Dtos.ReviewRestaurant;
using Restaurant.Domain.Restaurants;
using Restaurant.Domain.Restaurants.Enums;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Commands.ReviewRestaurant
{
    public sealed class ReviewRestaurantCommandHandler(
    IRestaurantRepository restaurantRepository)
    : IRequestHandler<ReviewRestaurantCommand, Result<ReviewRestaurantResponse>>
    {
        public async Task<Result<ReviewRestaurantResponse>> Handle(
            ReviewRestaurantCommand command,
            CancellationToken cancellationToken)
        {
            var request = command.Request;

            var restaurant = await restaurantRepository.GetByIdAsync(
                command.RestaurantId,
                cancellationToken);

            if (restaurant is null)
                return RestaurantErrors.NotFound;

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

            return new ReviewRestaurantResponse(
                restaurant.Id,
                request.Status.ToString(),
                $"Restaurant has been {request.Status.ToString().ToLower()}.");
        }
    }
}
