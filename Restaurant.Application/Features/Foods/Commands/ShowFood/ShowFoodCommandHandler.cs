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

namespace Restaurant.Application.Features.Foods.Commands.ShowFood
{
    public sealed class ShowFoodCommandHandler(IFoodRepository foodRepository,
        ICacheService cacheService, ILogger<ShowFoodCommandHandler> logger) : IRequestHandler<ShowFoodCommand, Result<Updated>>
    {
        public async Task<Result<Updated>> Handle(ShowFoodCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Processing ShowFoodCommand for FoodId: {FoodId}", request.FoodId);

            var food = await foodRepository.GetByIdAsync(request.FoodId, cancellationToken);
            if (food is null) return FoodErrors.NotFound;

            var result = food.Show();
            if (result.IsError) return result.TopError;

            await foodRepository.SaveChangesAsync(cancellationToken);
            await cacheService.RemoveByTagAsync($"food:{food.Id}", cancellationToken);
            await cacheService.RemoveByTagAsync($"restaurant:{food.RestaurantId}:menu", cancellationToken);
            await cacheService.RemoveByTagAsync("foods", cancellationToken);

            logger.LogInformation("Successfully processed ShowFoodCommand for FoodId: {FoodId}", request.FoodId);

            return Result.Updated;
        }
    }
}
