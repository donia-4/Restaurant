using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.Foods.Dtos.GetFoodById;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Queries.GetFoodById
{
    public sealed class GetFoodByIdQueryHandler(IFoodRepository foodRepository) : IRequestHandler<GetFoodByIdQuery, Result<FoodDetailsResponse>>
    {
        public async Task<Result<FoodDetailsResponse>> Handle(GetFoodByIdQuery request, CancellationToken cancellationToken)
        {
            var food = await foodRepository.GetByIdAsync(request.FoodId, cancellationToken);
            if (food is null) return FoodErrors.NotFound;
            return new FoodDetailsResponse(food.Id, food.RestaurantId, food.CategoryId, food.Name, food.Description, food.Price, food.Image, food.PreparationTimeMinutes, food.Calories, food.IsAvailable, food.IsVisible);
        }
    }
}
