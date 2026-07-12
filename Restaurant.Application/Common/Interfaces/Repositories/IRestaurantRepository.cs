using Restaurant.Domain.Restaurants;

namespace Restaurant.Application.Common.Interfaces.Repositories;

public interface IRestaurantRepository
{
    Task AddAsync(
        Domain.Restaurants.Restaurant restaurant,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}