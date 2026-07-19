using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Domain.Categories;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Categories.Commands.DeleteCategory
{
    public sealed class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, 
        ICacheService cacheService, ILogger<DeleteCategoryCommandHandler> logger) 
        : IRequestHandler<DeleteCategoryCommand, Result<Deleted>>
    {
        public async Task<Result<Deleted>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling DeleteCategoryCommand for CategoryId: {CategoryId}", request.CategoryId);

            var category = await categoryRepository.GetByIdWithFoodsAsync(request.CategoryId, cancellationToken);
            if (category is null) return CategoryErrors.NotFound;

            var canDelete = category.CanDelete();
            if (canDelete.IsError) return canDelete.TopError;

            categoryRepository.Remove(category);
            await categoryRepository.SaveChangesAsync(cancellationToken);

            await cacheService.RemoveByTagAsync($"category:{category.Id}", cancellationToken);
            await cacheService.RemoveByTagAsync($"restaurant:{category.RestaurantId}:categories", cancellationToken);
            await cacheService.RemoveByTagAsync("categories", cancellationToken);

            logger.LogInformation("Category with Id: {CategoryId} deleted successfully", request.CategoryId);

            return Result.Deleted;
        }
    }
}
