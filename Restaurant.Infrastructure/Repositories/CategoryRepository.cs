using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Domain.Categories;
using Restaurant.Infrastructure.Data;

namespace Restaurant.Infrastructure.Repositories;

public sealed class CategoryRepository(RestaurantDbContext context)
    : ICategoryRepository
{
    private readonly RestaurantDbContext _context = context;

    public async Task AddAsync(
        Category category,
        CancellationToken cancellationToken = default)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<Category?> GetByIdWithFoodsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Include(x => x.Foods)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> GetByRestaurantIdAsync(
        Guid restaurantId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .Include(x => x.Foods)
            .Where(x => x.RestaurantId == restaurantId)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public void Remove(Category category)
    {
        category.IsDeleted = true;
        _context.Categories.Update(category);   
    }
    public async Task<bool> ExistsWithTheGivenName(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Categories.AnyAsync(c => c.Name.ToLower() == name, cancellationToken);
    }

    public IQueryable<Category> GetByRestaurantId(Guid restaurantId)
    {
        return _context.Categories
            .AsNoTracking()
            .Include(c => c.Foods)
            .Where(c => c.RestaurantId == restaurantId)
            .OrderBy(c => c.DisplayOrder);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}