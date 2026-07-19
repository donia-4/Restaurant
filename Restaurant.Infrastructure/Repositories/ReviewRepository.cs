using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Models;
using Restaurant.Domain.Reviews;
using Restaurant.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Infrastructure.Repositories
{
    public sealed class ReviewRepository(RestaurantDbContext context)
    : IReviewRepository
    {
        private readonly RestaurantDbContext _context = context;

        public async Task<Review?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    r => r.Id == id && !r.IsDeleted,
                    cancellationToken);
        }

        public async Task<Review?> GetByIdWithUserAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .AsNoTracking()
                .Include(r => r.Restaurant)
                .FirstOrDefaultAsync(
                    r => r.Id == id && !r.IsDeleted,
                    cancellationToken);
        }

        public async Task<Review?> GetByUserAndRestaurantAsync(
            Guid userId,
            Guid restaurantId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    r => r.UserId == userId
                         && r.RestaurantId == restaurantId
                         && !r.IsDeleted,
                    cancellationToken);
        }

        public async Task<PaginatedList<Review>> GetByRestaurantAsync(
            Guid restaurantId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Reviews
                .AsNoTracking()
                .Where(r => r.RestaurantId == restaurantId && !r.IsDeleted)
                .OrderByDescending(r => r.CreatedAtUtc);

            return await PaginatedList<Review>.CreateAsync(
                query,
                pageNumber,
                pageSize,
                cancellationToken);
        }

        public async Task<PaginatedList<Review>> GetByUserAsync(
            Guid userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Reviews
                .AsNoTracking()
                .Where(r => r.UserId == userId && !r.IsDeleted)
                .OrderByDescending(r => r.CreatedAtUtc);

            return await PaginatedList<Review>.CreateAsync(
                query,
                pageNumber,
                pageSize,
                cancellationToken);
        }

        public async Task AddAsync(
            Review review,
            CancellationToken cancellationToken = default)
        {
            await _context.Reviews.AddAsync(review, cancellationToken);
        }

        public void Update(Review review)
        {
            _context.Reviews.Update(review);
        }

        public void Remove(Review review)
        {
            review.IsDeleted = true;
            _context.Reviews.Update(review);
        }

        public async Task SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

}
