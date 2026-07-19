using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Commands.HideFood
{
    public sealed class HideFoodCommandHandler(IFoodRepository foodRepository,
        ICacheService cacheService, ILogger<HideFoodCommandHandler> logger) : IRequestHandler<HideFoodCommand, Result<Updated>>
    {
        public async Task<Result<Updated>> Handle(HideFoodCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling HideFoodCommand for FoodId: {FoodId}", request.FoodId);

            var food = await foodRepository.GetByIdAsync(request.FoodId, cancellationToken);
            if (food is null) return FoodErrors.NotFound;

            var result = food.Hide();
            if (result.IsError) return result.TopError;

            await foodRepository.SaveChangesAsync(cancellationToken);
            await cacheService.RemoveByTagAsync($"food:{food.Id}", cancellationToken);
            await cacheService.RemoveByTagAsync($"restaurant:{food.RestaurantId}:menu", cancellationToken);
            await cacheService.RemoveByTagAsync("foods", cancellationToken);

            logger.LogInformation("Food with FoodId: {FoodId} has been hidden successfully.", request.FoodId);
            return Result.Updated;
        }
    }
}
