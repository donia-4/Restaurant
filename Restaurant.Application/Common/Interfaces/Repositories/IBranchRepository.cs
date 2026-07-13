using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Domain.Branches;

namespace Restaurant.Application.Common.Interfaces.Repositories
{
    public interface IBranchRepository
    {
        Task AddAsync(
            Branch branch,
            CancellationToken cancellationToken = default);

        Task<Branch?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<Branch?> GetByIdWithWorkingHoursAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Branch>> GetByRestaurantIdAsync(Guid restaurantId, CancellationToken cancellationToken = default);
        void Remove(Branch branch);

        Task SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}
