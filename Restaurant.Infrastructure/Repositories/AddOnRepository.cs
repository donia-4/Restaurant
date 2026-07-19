using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Domain.AddOns;
using Microsoft.EntityFrameworkCore;
using Restaurant.Infrastructure.Data;

namespace Restaurant.Infrastructure.Repositories
{
    public sealed class AddOnRepository(RestaurantDbContext context)
    : IAddOnRepository
    {
        private readonly RestaurantDbContext _context = context;

        public async Task<AddOn?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _context.AddOns
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }

        public async Task AddAsync(
            AddOn addOn,
            CancellationToken cancellationToken = default)
        {
            await _context.AddOns.AddAsync(addOn, cancellationToken);
        }

        public void Remove(AddOn addOn)
        {
            addOn.IsDeleted = true;
            _context.AddOns.Update(addOn);
        }

        public async Task SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
