using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.Restaurants.Dtos.RequestRestaurantModification;
using Restaurant.Domain.Restaurants;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Commands.RequestRestaurantModification;

public sealed class RequestRestaurantModificationCommandHandler(
    IRestaurantRepository restaurantRepository)
    : IRequestHandler<RequestRestaurantModificationCommand,
        Result<RequestRestaurantModificationResponse>>
{
    private readonly IRestaurantRepository _restaurantRepository =
        restaurantRepository;

    public async Task<Result<RequestRestaurantModificationResponse>> Handle(
        RequestRestaurantModificationCommand request,
        CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(
            request.RestaurantId,
            cancellationToken);

        if (restaurant is null)
        {
            return RestaurantErrors.NotFound;
        }

        var result = restaurant.RequestModification();

        if (result.IsError)
        {
            return result.Errors;
        }

        await _restaurantRepository.SaveChangesAsync(cancellationToken);

        return new RequestRestaurantModificationResponse(
            restaurant.Id,
            restaurant.Name,
            restaurant.Status.ToString(),
            restaurant.IsApproved);
    }
}