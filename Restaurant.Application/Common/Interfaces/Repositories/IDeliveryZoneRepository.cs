using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Domain.DeliveryZones;

namespace Restaurant.Application.Common.Interfaces.Repositories
{
    public interface IDeliveryZoneRepository
    {
        Task AddAsync(
            DeliveryZone zone,
            CancellationToken cancellationToken = default);

        Task<DeliveryZone?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<DeliveryZone>> GetByBranchIdAsync(
            Guid branchId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            Guid branchId,
            string zoneName,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(
            DeliveryZone zone,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            DeliveryZone zone,
            CancellationToken cancellationToken = default);

        Task SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}  
