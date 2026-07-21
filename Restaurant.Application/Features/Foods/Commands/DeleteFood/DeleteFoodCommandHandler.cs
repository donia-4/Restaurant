using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Results;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Restaurant.Application.Features.Foods.Commands.DeleteFood;

public sealed class DeleteFoodCommandHandler(
    IFoodRepository foodRepository,
    ICacheService cacheService,
    IFileService fileService,
    ILogger<DeleteFoodCommandHandler> logger)
    : IRequestHandler<DeleteFoodCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(
        DeleteFoodCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Processing DeleteFoodCommand for Food ID: {FoodId}",
            request.FoodId);

        var food = await foodRepository.GetByIdAsync(
            request.FoodId,
            cancellationToken);

        if (food is null)
        {
            return FoodErrors.NotFound;
        }

        if (!string.IsNullOrWhiteSpace(food.ImagePublicId))
        {
            await fileService.DeleteAsync(
                food.ImagePublicId,
                cancellationToken);
        }

        foodRepository.Remove(food);
        await foodRepository.SaveChangesAsync(cancellationToken);

        await cacheService.RemoveByTagAsync(
            $"food:{food.Id}",
            cancellationToken);

        await cacheService.RemoveByTagAsync(
            $"restaurant:{food.RestaurantId}:menu",
            cancellationToken);

        await cacheService.RemoveByTagAsync(
            $"category:{food.CategoryId}:foods",
            cancellationToken);

        await cacheService.RemoveByTagAsync(
            "foods",
            cancellationToken);

        logger.LogInformation(
            "Successfully deleted Food with ID: {FoodId}",
            request.FoodId);

        return Result.Deleted;
    }
}