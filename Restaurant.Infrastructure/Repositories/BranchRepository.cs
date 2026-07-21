using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Domain.Branches;
using Microsoft.EntityFrameworkCore;
using Restaurant.Infrastructure.Data;

namespace Restaurant.Infrastructure.Repositories
{
    public sealed class BranchRepository(RestaurantDbContext context)
    : IBranchRepository
    {
        private readonly RestaurantDbContext _context = context;

        public async Task AddAsync(
            Branch branch,
            CancellationToken cancellationToken = default)
        {
            await _context.Branches.AddAsync(branch, cancellationToken);
        }

        public async Task<Branch?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Branches
                .Include(x => x.WorkingHours)
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }

        public async Task<Branch?> GetByIdWithWorkingHoursAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Branches
                .Include(x => x.WorkingHours)
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }

        public async Task<IReadOnlyList<Branch>> GetByRestaurantIdAsync(Guid restaurantId, CancellationToken cancellationToken = default)
        {
            return await _context.Branches
                .AsNoTracking()
                .Include(x => x.WorkingHours)
                .Where(x => x.RestaurantId == restaurantId)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);
        }

        public void Remove(Branch branch)
        {
            branch.IsDeleted = true;
            _context.Branches.Update(branch);
        }
        public async Task<bool> ExistsWithTheGivenName(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Branches.AnyAsync(b => b.Name.ToLower() == name, cancellationToken);
        }

        public IQueryable<Branch> GetByRestaurantId(Guid restaurantId)
        {
            return _context.Branches
                .AsNoTracking()
                .Include(b => b.WorkingHours)
                .Where(b => b.RestaurantId == restaurantId);
        }

        public async Task SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
