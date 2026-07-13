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

namespace Restaurant.Application.Features.Foods.Commands.DeleteFood
{
    public sealed class DeleteFoodCommandHandler(IFoodRepository foodRepository, ICacheService cacheService) : IRequestHandler<DeleteFoodCommand, Result<Deleted>>
    {
        public async Task<Result<Deleted>> Handle(DeleteFoodCommand request, CancellationToken cancellationToken)
        {
            var food = await foodRepository.GetByIdAsync(request.FoodId, cancellationToken);
            if (food is null) return FoodErrors.NotFound;

            foodRepository.Remove(food);
            await foodRepository.SaveChangesAsync(cancellationToken);

            await cacheService.RemoveByTagAsync($"food:{food.Id}", cancellationToken);
            await cacheService.RemoveByTagAsync($"restaurant:{food.RestaurantId}:menu", cancellationToken);
            await cacheService.RemoveByTagAsync($"category:{food.CategoryId}:foods", cancellationToken);
            await cacheService.RemoveByTagAsync("foods", cancellationToken);
            return Result.Deleted;
        }
    }
}
