using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Domain.Categories;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Categories.Commands.ReorderCategories
{
    public sealed class ReorderCategoriesCommandHandler(ICategoryRepository categoryRepository, ICacheService cacheService) : IRequestHandler<ReorderCategoriesCommand, Result<Updated>>
    {
        public async Task<Result<Updated>> Handle(ReorderCategoriesCommand request, CancellationToken cancellationToken)
        {
            Guid restaurantId = Guid.Empty;
            foreach (var item in request.Items)
            {
                var category = await categoryRepository.GetByIdAsync(item.CategoryId, cancellationToken);
                if (category is null) return CategoryErrors.NotFound;
                var result = category.Reorder(item.DisplayOrder);
                if (result.IsError) return result.TopError;
                restaurantId = category.RestaurantId;
            }
            await categoryRepository.SaveChangesAsync(cancellationToken);
            if (restaurantId != Guid.Empty) await cacheService.RemoveByTagAsync($"restaurant:{restaurantId}:categories", cancellationToken);
            await cacheService.RemoveByTagAsync("categories", cancellationToken);
            return Result.Updated;
        }
    }
}
