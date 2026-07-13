using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Application.Features.Foods.Dtos.UpdateFood;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Commands.UpdateFood
{
    public sealed class UpdateFoodCommandHandler(IFoodRepository foodRepository, ICacheService cacheService) : IRequestHandler<UpdateFoodCommand, Result<UpdateFoodResponse>>
    {
        public async Task<Result<UpdateFoodResponse>> Handle(UpdateFoodCommand command, CancellationToken cancellationToken)
        {
            var food = await foodRepository.GetByIdAsync(command.FoodId, cancellationToken);
            if (food is null) return FoodErrors.NotFound;

            var result = food.Update(command.Request.Name, command.Request.Description, command.Request.Price, command.Request.CategoryId, command.Request.Image, command.Request.PreparationTimeMinutes, command.Request.Calories);
            if (result.IsError) return result.TopError;

            await foodRepository.SaveChangesAsync(cancellationToken);
            await cacheService.RemoveByTagAsync($"food:{food.Id}", cancellationToken);
            await cacheService.RemoveByTagAsync($"restaurant:{food.RestaurantId}:menu", cancellationToken);
            await cacheService.RemoveByTagAsync("foods", cancellationToken);
            return new UpdateFoodResponse(food.Id);
        }
    }
}
