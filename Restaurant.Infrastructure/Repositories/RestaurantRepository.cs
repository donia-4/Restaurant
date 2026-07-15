using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Domain.Restaurants.Enums;
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
            .Include(restaurant => restaurant.Branches)
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
    public IQueryable<Domain.Restaurants.Restaurant> Search(
    string? name,
    string? city,
    CuisineType? cuisineType,
    Guid? categoryId,
    RestaurantStatus? status,
    decimal? minRating)
    {
        var query = _context.Restaurants
            .Include(r => r.Categories)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(r => r.Name.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(r => r.Address.Contains(city));
        }

        if (cuisineType.HasValue)
        {
            query = query.Where(r => r.CuisineType == cuisineType.Value);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(r => r.Categories.Any(c => c.Id == categoryId.Value));
        }

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        if (minRating.HasValue)
        {
            query = query.Where(r => r.AverageRating >= minRating.Value);
        }

        return query;
    }
    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}