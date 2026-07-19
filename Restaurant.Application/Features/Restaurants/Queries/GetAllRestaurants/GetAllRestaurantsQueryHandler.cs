using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.Restaurants.Dtos.GetAllRestaurants;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Queries.GetAllRestaurants;

public sealed class GetAllRestaurantsQueryHandler(
    IRestaurantRepository restaurantRepository,
    ILogger<GetAllRestaurantsQueryHandler> logger)
    : IRequestHandler<
        GetAllRestaurantsQuery,
        Result<PaginatedList<GetAllRestaurantsResponse>>>
{
    private readonly IRestaurantRepository _restaurantRepository =
        restaurantRepository;

    public async Task<Result<PaginatedList<GetAllRestaurantsResponse>>> Handle(
        GetAllRestaurantsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Processing GetAllRestaurantsQuery. PageNumber: {PageNumber}, PageSize: {PageSize}",
            request.PageNumber,
            request.PageSize);

        var restaurants = await _restaurantRepository.GetAllAsync(
            cancellationToken);

        var mappedRestaurants = restaurants
            .Select(restaurant => new GetAllRestaurantsResponse(
                restaurant.Id,
                restaurant.Name,
                restaurant.Description,
                restaurant.Logo,
                restaurant.CoverImage,
                restaurant.Phone,
                restaurant.Email,
                restaurant.CuisineType,
                restaurant.Address,
                restaurant.Status,
                restaurant.IsApproved,
                restaurant.AverageRating,
                restaurant.TotalReviews))
            .ToList();

        var paginatedResult = new PaginatedList<GetAllRestaurantsResponse>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = mappedRestaurants.Count,
            TotalPages = (int)Math.Ceiling(mappedRestaurants.Count / (double)request.PageSize),
            Items = mappedRestaurants
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList()
        };

        logger.LogInformation(
            "Returning {ReturnedCount} restaurants out of {TotalCount}",
            paginatedResult.Items.Count,
            paginatedResult.TotalCount);

        return paginatedResult;
    }
}