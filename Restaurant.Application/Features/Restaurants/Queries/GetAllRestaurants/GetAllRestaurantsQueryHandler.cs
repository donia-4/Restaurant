using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.Restaurants.Dtos.GetAllRestaurants;
using Restaurant.Domain.Results;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Application.Features.Restaurants.Queries.GetAllRestaurants
{
    public sealed class GetAllRestaurantsQueryHandler(
    IRestaurantRepository restaurantRepository)
    : IRequestHandler<
        GetAllRestaurantsQuery,
        Result<List<GetAllRestaurantsResponse>>>
    {
        private readonly IRestaurantRepository _restaurantRepository =
            restaurantRepository;

        public async Task<Result<List<GetAllRestaurantsResponse>>> Handle(
            GetAllRestaurantsQuery request,
            CancellationToken cancellationToken)
        {
            var restaurants = await _restaurantRepository.GetAllAsync(
                cancellationToken);

            var response = restaurants
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
                    restaurant.IsApproved))
                .ToList();

            return response;
        }
    }
}
