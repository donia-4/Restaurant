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
    public sealed class UpdateFoodCommandHandler(
    IFoodRepository foodRepository,
    ICacheService cacheService)
    : IRequestHandler<UpdateFoodCommand, Result<UpdateFoodResponse>>
    {
        private readonly IFoodRepository _foodRepository = foodRepository;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<Result<UpdateFoodResponse>> Handle(
            UpdateFoodCommand command,
            CancellationToken cancellationToken)
        {
            var request = command.Request;

            var food = await _foodRepository.GetByIdAsync(
                command.FoodId,
                cancellationToken);

            if (food is null)
            {
                return FoodErrors.NotFound;
            }

            var result = food.Update(
                request.Name ?? food.Name,
                request.Description ?? food.Description,
                request.Price ?? food.Price,
                request.CategoryId ?? food.CategoryId,
                request.Image ?? food.Image,
                request.PreparationTimeMinutes ?? food.PreparationTimeMinutes,
                request.Calories ?? food.Calories);

            if (result.IsError)
            {
                return result.TopError;
            }

            await _foodRepository.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveByTagAsync($"food:{food.Id}", cancellationToken);
            await _cacheService.RemoveByTagAsync($"restaurant:{food.RestaurantId}:menu", cancellationToken);
            await _cacheService.RemoveByTagAsync("foods", cancellationToken);

            return new UpdateFoodResponse(food.Id);
        }
    }
}
