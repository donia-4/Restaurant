using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Application.Features.Restaurants.Dtos.RejectRestaurant;
using Restaurant.Domain.Restaurants;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Commands.RejectRestaurant;

public sealed class RejectRestaurantCommandHandler(
    IRestaurantRepository restaurantRepository, ICacheService _cacheService)
    : IRequestHandler<RejectRestaurantCommand, Result<RejectRestaurantResponse>>
{
    private readonly IRestaurantRepository _restaurantRepository =
        restaurantRepository;

    public async Task<Result<RejectRestaurantResponse>> Handle(
        RejectRestaurantCommand request,
        CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(
            request.RestaurantId,
            cancellationToken);

        if (restaurant is null)
        {
            return RestaurantErrors.NotFound;
        }

        var result = restaurant.Reject(request.Reason);

        if (result.IsError)
        {
            return result.Errors;
        }

        await _restaurantRepository.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByTagAsync(
            "restaurants",
            cancellationToken);

        return new RejectRestaurantResponse(
            restaurant.Id,
            restaurant.Name,
            restaurant.Status.ToString(),
            restaurant.IsApproved);
    }
}