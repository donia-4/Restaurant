using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Domain.Foods;
using Restaurant.Infrastructure.Data;

namespace Restaurant.Infrastructure.Repositories;

public sealed class FoodRepository(RestaurantDbContext context)
    : IFoodRepository
{
    private readonly RestaurantDbContext _context = context;

    public async Task AddAsync(
        Food food,
        CancellationToken cancellationToken = default)
    {
        await _context.Foods.AddAsync(food, cancellationToken);
    }

    public async Task<Food?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Foods
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<Food?> GetByIdWithAddOnsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Foods
            //.AsNoTracking()
            .Include(x => x.AddOns)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Food>> GetByRestaurantIdAsync(
        Guid restaurantId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Foods
            //.AsNoTracking()
            .Where(x => x.RestaurantId == restaurantId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Food>> GetByCategoryIdAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Foods
            //.AsNoTracking()
            .Where(x => x.CategoryId == categoryId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public void Remove(Food food)
    {
        _context.Foods.Remove(food);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}