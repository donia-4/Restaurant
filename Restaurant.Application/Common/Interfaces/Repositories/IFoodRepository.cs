using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Domain.Foods;

namespace Restaurant.Application.Common.Interfaces.Repositories
{
    public interface IFoodRepository
    {
        Task AddAsync(Food food, CancellationToken cancellationToken = default);
        Task<Food?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Food?> GetByIdWithAddOnsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Food>> GetByRestaurantIdAsync(Guid restaurantId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Food>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
        void Remove(Food food);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
