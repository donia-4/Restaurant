using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Application.Features.Categories.Dtos.UpdateCategory;
using Restaurant.Domain.Categories;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Categories.Commands.UpdateCategory
{
    public sealed class UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, ICacheService cacheService) : IRequestHandler<UpdateCategoryCommand, Result<UpdateCategoryResponse>>
    {
        public async Task<Result<UpdateCategoryResponse>> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);
            if (category is null) return CategoryErrors.NotFound;

            var result = category.Rename(command.Request.Name);
            if (result.IsError) return result.TopError;

            await categoryRepository.SaveChangesAsync(cancellationToken);
            await cacheService.RemoveByTagAsync($"category:{category.Id}", cancellationToken);
            await cacheService.RemoveByTagAsync($"restaurant:{category.RestaurantId}:categories", cancellationToken);
            await cacheService.RemoveByTagAsync("categories", cancellationToken);
            return new UpdateCategoryResponse(category.Id);
        }
    }
}
