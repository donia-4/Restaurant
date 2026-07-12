using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Infrastructure.Data;

namespace Restaurant.Infrastructure.Repositories;

public sealed class RestaurantRepository(RestaurantDbContext context)
    : IRestaurantRepository
{
    private readonly RestaurantDbContext _context = context;

    public async Task AddAsync(
        Domain.Restaurants.Restaurant restaurant,
        CancellationToken cancellationToken = default)
    {
        await _context.Restaurants.AddAsync(
            restaurant,
            cancellationToken);
    }

    public async Task<Domain.Restaurants.Restaurant?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Restaurants
            .FirstOrDefaultAsync(
                restaurant => restaurant.Id == id,
                cancellationToken);
    }

    public async Task<List<Domain.Restaurants.Restaurant>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Restaurants
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}