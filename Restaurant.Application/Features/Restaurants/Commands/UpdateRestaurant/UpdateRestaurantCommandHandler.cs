using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Application.Features.Restaurants.Dtos.UpdateRestaurant;
using Restaurant.Domain.Restaurants;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Commands.UpdateRestaurant;

public sealed class UpdateRestaurantCommandHandler(
    IRestaurantRepository restaurantRepository,
    ICacheService cacheService)
    : IRequestHandler<
        UpdateRestaurantCommand,
        Result<UpdateRestaurantResponse>>
{
    private readonly IRestaurantRepository _restaurantRepository =
        restaurantRepository;

    private readonly ICacheService _cacheService =
        cacheService;

    public async Task<Result<UpdateRestaurantResponse>> Handle(
        UpdateRestaurantCommand request,
        CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(
            request.RestaurantId,
            cancellationToken);

        if (restaurant is null)
        {
            return RestaurantErrors.NotFound;
        }

        var result = restaurant.UpdateDetails(
            request.Name,
            request.Description,
            request.Phone,
            request.Email,
            request.CuisineType,
            request.Address);

        if (result.IsError)
        {
            return result.Errors;
        }

        await _restaurantRepository.SaveChangesAsync(
            cancellationToken);

        await _cacheService.RemoveByTagAsync(
            "restaurants",
            cancellationToken);

        return new UpdateRestaurantResponse(
            restaurant.Id,
            restaurant.Name,
            restaurant.Description,
            restaurant.Phone,
            restaurant.Email,
            restaurant.CuisineType,
            restaurant.Address);
    }
}