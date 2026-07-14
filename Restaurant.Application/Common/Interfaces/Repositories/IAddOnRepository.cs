using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Domain.AddOns;

namespace Restaurant.Application.Common.Interfaces.Repositories
{
    public interface IAddOnRepository
    {
        Task<AddOn?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            AddOn addOn,
            CancellationToken cancellationToken = default);

        void Remove(AddOn addOn);

        Task SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}
