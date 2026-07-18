using MediatR;
using Restaurant.Application.Common.Dtos;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Application.Features.Restaurants.Commands.CreateRestaurant;
using Restaurant.Application.Features.Restaurants.Dtos.CreateRestaurant;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.CreateRestaurant;

public sealed class CreateRestaurantCommandHandler(
    IRestaurantRepository restaurantRepository,
    IFileService fileService)
    : IRequestHandler<
        CreateRestaurantCommand,
        Result<CreateRestaurantResponse>>
{
    public async Task<Result<CreateRestaurantResponse>> Handle(
        CreateRestaurantCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        UploadFileResponse? logo = null;
        if (request.Logo is not null)
        {
            logo = await fileService.UploadAsync(
                request.Logo,
                cancellationToken);
        }

        UploadFileResponse? cover = null;
        if (request.CoverImage is not null)
        {
            cover = await fileService.UploadAsync(
                request.CoverImage,
                cancellationToken);
        }

        var restaurantResult =
            Domain.Restaurants.Restaurant.Create(
                Guid.NewGuid(),
                request.OwnerId,
                request.Name,
                request.Description,
                request.Phone,
                request.Email,
                request.CuisineType,
                request.Address,
                logo?.Url,
                logo?.PublicId,
                cover?.Url,
                cover?.PublicId);

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
            restaurant.Logo,
            restaurant.CoverImage,
            restaurant.Status.ToString(),
            restaurant.IsApproved);
    }
}