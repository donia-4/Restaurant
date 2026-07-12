using Restaurant.Domain.Restaurants;

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
    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}