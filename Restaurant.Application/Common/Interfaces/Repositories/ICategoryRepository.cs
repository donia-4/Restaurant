using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Domain.Categories;

namespace Restaurant.Application.Common.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category, CancellationToken cancellationToken = default);
        Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Category?> GetByIdWithFoodsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Category>> GetByRestaurantIdAsync(Guid restaurantId, CancellationToken cancellationToken = default);
        void Remove(Category category);
        Task<bool> ExistsWithTheGivenName(string name, CancellationToken cancellationToken = default);
        IQueryable<Category> GetByRestaurantId(Guid restaurantId); 
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }

}
