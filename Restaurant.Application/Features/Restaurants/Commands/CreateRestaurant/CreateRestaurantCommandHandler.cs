using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.Restaurants.Commands.CreateRestaurant;
using Restaurant.Application.Features.Restaurants.Dtos.CreateRestaurant;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.CreateRestaurant;

public sealed class CreateRestaurantCommandHandler(
    IRestaurantRepository restaurantRepository)
    : IRequestHandler<
        CreateRestaurantCommand,
        Result<CreateRestaurantResponse>>
{
    public async Task<Result<CreateRestaurantResponse>> Handle(
        CreateRestaurantCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        var restaurantResult =
            Domain.Restaurants.Restaurant.Create(
                Guid.NewGuid(),
                request.OwnerId,
                request.Name,
                request.Description,
                request.Phone,
                request.Email,
                request.CuisineType,
                request.Address);

        if (restaurantResult.IsError)
        {
            return restaurantResult.Errors;
        }

        var restaurant = restaurantResult.Value;

        await restaurantRepository.AddAsync(
            restaurant,
            cancellationToken);

        await restaurantRepository.SaveChangesAsync(
            cancellationToken);

        return new CreateRestaurantResponse(
            restaurant.Id,
            restaurant.OwnerId,
            restaurant.Name,
            restaurant.Description,
            restaurant.Phone,
            restaurant.Email,
            restaurant.CuisineType,
            restaurant.Address,
            restaurant.Status.ToString(),
            restaurant.IsApproved);
    }
}