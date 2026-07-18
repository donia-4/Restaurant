using MediatR;
using Restaurant.Application.Common.Dtos;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Application.Features.Restaurants.Commands.UpdateRestaurant;
using Restaurant.Application.Features.Restaurants.Dtos.UpdateRestaurant;
using Restaurant.Domain.Restaurants;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.UpdateRestaurant;

public sealed class UpdateRestaurantCommandHandler(
    IRestaurantRepository restaurantRepository,
    ICacheService cacheService,
    IFileService fileService)
    : IRequestHandler<
        UpdateRestaurantCommand,
        Result<UpdateRestaurantResponse>>
{
    private readonly IRestaurantRepository _restaurantRepository =
        restaurantRepository;

    private readonly ICacheService _cacheService =
        cacheService;

    private readonly IFileService _fileService =
        fileService;

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

        UploadFileResponse? logo = null;
        if (request.Logo is not null)
        {
            logo = await _fileService.UploadAsync(
                request.Logo,
                cancellationToken);
        }

        UploadFileResponse? cover = null;
        if (request.CoverImage is not null)
        {
            cover = await _fileService.UploadAsync(
                request.CoverImage,
                cancellationToken);
        }

        var result = restaurant.UpdateDetails(
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
            restaurant.Address,
            restaurant.Logo,
            restaurant.CoverImage);
    }
}