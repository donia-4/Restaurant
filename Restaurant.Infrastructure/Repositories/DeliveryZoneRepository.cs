using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Domain.DeliveryZones;
using Restaurant.Infrastructure.Data;

namespace Restaurant.Infrastructure.Repositories
{
    public sealed class DeliveryZoneRepository(RestaurantDbContext context)
    : IDeliveryZoneRepository
    {
        private readonly RestaurantDbContext _context = context;

        public async Task AddAsync(
            DeliveryZone zone,
            CancellationToken cancellationToken = default)
        {
            await _context.DeliveryZones.AddAsync(zone, cancellationToken);
        }

        public async Task<DeliveryZone?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _context.DeliveryZones
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }

        public async Task<List<DeliveryZone>> GetByBranchIdAsync(
            Guid branchId,
            CancellationToken cancellationToken = default)
        {
            return await _context.DeliveryZones
                .AsNoTracking()
                .Where(x => x.BranchId == branchId)
                .OrderBy(x => x.ZoneName)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(
            Guid branchId,
            string zoneName,
            CancellationToken cancellationToken = default)
        {
            return await _context.DeliveryZones
                .AnyAsync(
                    x => x.BranchId == branchId && x.ZoneName == zoneName,
                    cancellationToken);
        }

        public Task UpdateAsync(
            DeliveryZone zone,
            CancellationToken cancellationToken = default)
        {
            _context.DeliveryZones.Update(zone);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(
            DeliveryZone zone,
            CancellationToken cancellationToken = default)
        {
            _context.DeliveryZones.Remove(zone);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
