using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.Foods.Dtos.GetRestaurantMenu;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Queries.GetRestaurantMenu
{
    public sealed class GetRestaurantMenuQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetRestaurantMenuQuery, Result<IReadOnlyList<MenuCategoryResponse>>>
    {
        public async Task<Result<IReadOnlyList<MenuCategoryResponse>>> Handle(GetRestaurantMenuQuery request, CancellationToken cancellationToken)
        {
            var categories = await categoryRepository.GetByRestaurantIdAsync(request.RestaurantId, cancellationToken);
            return categories.OrderBy(c => c.DisplayOrder).Select(c => new MenuCategoryResponse(c.Id, c.Name, c.DisplayOrder,
                c.Foods.Where(f => f.IsVisible).Select(f => new MenuFoodResponse(f.Id, f.Name, f.Description, f.Price, f.Image, f.PreparationTimeMinutes, f.Calories, f.IsAvailable)).ToList())).ToList();
        }
    }
}
