using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Application.Features.Restaurants.Dtos.ApproveRestaurant;
using Restaurant.Domain.Restaurants;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Commands.ApproveRestaurant;

public sealed class ApproveRestaurantCommandHandler(
    IRestaurantRepository restaurantRepository, ICacheService _cacheService)
    : IRequestHandler<ApproveRestaurantCommand, Result<ApproveRestaurantResponse>>
{
    private readonly IRestaurantRepository _restaurantRepository =
        restaurantRepository;

    public async Task<Result<ApproveRestaurantResponse>> Handle(
        ApproveRestaurantCommand request,
        CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(
            request.RestaurantId,
            cancellationToken);

        if (restaurant is null)
        {
            return RestaurantErrors.NotFound;
        }

        var result = restaurant.Approve();

        if (result.IsError)
        {
            return result.Errors;
        }

        await _restaurantRepository.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByTagAsync(
            "restaurants",
            cancellationToken);

        return new ApproveRestaurantResponse(
            restaurant.Id,
            restaurant.Name,
            restaurant.Status.ToString(),
            restaurant.IsApproved);
    }
}