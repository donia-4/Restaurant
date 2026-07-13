using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Commands.ChangeFoodAvailability
{
    public sealed class ChangeFoodAvailabilityCommandHandler(IFoodRepository foodRepository, ICacheService cacheService) : IRequestHandler<ChangeFoodAvailabilityCommand, Result<Updated>>
    {
        public async Task<Result<Updated>> Handle(ChangeFoodAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var food = await foodRepository.GetByIdAsync(request.FoodId, cancellationToken);
            if (food is null) return FoodErrors.NotFound;

            var result = food.ChangeAvailability(request.IsAvailable);
            if (result.IsError) return result.TopError;

            await foodRepository.SaveChangesAsync(cancellationToken);
            await cacheService.RemoveByTagAsync($"food:{food.Id}", cancellationToken);
            await cacheService.RemoveByTagAsync($"restaurant:{food.RestaurantId}:menu", cancellationToken);
            await cacheService.RemoveByTagAsync("foods", cancellationToken);
            return Result.Updated;
        }
    }
}
