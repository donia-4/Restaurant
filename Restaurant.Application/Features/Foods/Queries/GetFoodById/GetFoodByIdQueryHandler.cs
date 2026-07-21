using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.Foods.Dtos.GetFoodById;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Queries.GetFoodById
{
    public sealed class GetFoodByIdQueryHandler(IFoodRepository foodRepository, ILogger<GetFoodByIdQueryHandler> logger) : IRequestHandler<GetFoodByIdQuery, Result<FoodDetailsResponse>>
    {
        public async Task<Result<FoodDetailsResponse>> Handle(GetFoodByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Processing GetFoodByIdQuery for Food ID: {FoodId}", request.FoodId);
            var food = await foodRepository.GetByIdAsync(request.FoodId, cancellationToken);
            if (food is null) return FoodErrors.NotFound;
            logger.LogInformation("Food found: {FoodName} (ID: {FoodId})", food.Name, food.Id);
            return new FoodDetailsResponse(food.Id, food.RestaurantId, food.CategoryId, food.Name, food.Description, food.Price, food.Image, food.PreparationTimeMinutes, food.Calories, food.IsAvailable, food.IsVisible);
        }
    }
}
