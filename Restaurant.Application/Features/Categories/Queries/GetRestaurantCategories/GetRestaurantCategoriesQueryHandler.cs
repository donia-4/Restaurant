using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.Categories.Dtos.GetRestaurantCategories;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Categories.Queries.GetRestaurantCategories
{
    public sealed class GetRestaurantCategoriesQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetRestaurantCategoriesQuery, Result<IReadOnlyList<CategoryResponse>>>
    {
        public async Task<Result<IReadOnlyList<CategoryResponse>>> Handle(GetRestaurantCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await categoryRepository.GetByRestaurantIdAsync(request.RestaurantId, cancellationToken);
            return categories.Select(c => new CategoryResponse(c.Id, c.RestaurantId, c.Name, c.DisplayOrder, c.Foods.Count)).ToList();
        }
    }
}
