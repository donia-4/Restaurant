using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Common.Models;
using Restaurant.Domain.Reviews;

namespace Restaurant.Application.Common.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Review?> GetByIdWithUserAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Review?> GetByUserAndRestaurantAsync(
            Guid userId,
            Guid restaurantId,
            CancellationToken cancellationToken = default);

        Task<PaginatedList<Review>> GetByRestaurantAsync(
            Guid restaurantId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<PaginatedList<Review>> GetByUserAsync(
            Guid userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task AddAsync(Review review, CancellationToken cancellationToken = default);

        void Update(Review review);

        void Remove(Review review);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
