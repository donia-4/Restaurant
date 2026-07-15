using Restaurant.Domain.Restaurants;
using Restaurant.Domain.Restaurants.Enums;

namespace Restaurant.Application.Common.Interfaces.Repositories;

public interface IRestaurantRepository
{
    Task AddAsync(
        Domain.Restaurants.Restaurant restaurant,
        CancellationToken cancellationToken = default);

    Task<Domain.Restaurants.Restaurant?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<List<Domain.Restaurants.Restaurant>> GetAllAsync(CancellationToken cancellationToken = default);

    IQueryable<Domain.Restaurants.Restaurant> Search(
        string? name,
        string? city,
        CuisineType? cuisineType,
        Guid? categoryId,
        RestaurantStatus? status,
        decimal? minRating);
    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

}