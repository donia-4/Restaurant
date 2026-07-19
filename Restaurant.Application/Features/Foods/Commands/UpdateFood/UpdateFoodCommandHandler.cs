using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Dtos;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Application.Features.Foods.Commands.UpdateFood;
using Restaurant.Application.Features.Foods.Dtos.UpdateFood;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Restaurants;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Commands.UpdateFood;

public sealed class UpdateFoodCommandHandler(
    IFoodRepository foodRepository,
    ICacheService cacheService,
    IFileService fileService,
    ILogger<UpdateFoodCommandHandler> logger)
    : IRequestHandler<UpdateFoodCommand, Result<UpdateFoodResponse>>
{
    private readonly IFoodRepository _foodRepository = foodRepository;
    private readonly ICacheService _cacheService = cacheService;
    private readonly IFileService _fileService = fileService;

    public async Task<Result<UpdateFoodResponse>> Handle(
        UpdateFoodCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Processing UpdateFoodCommand for Food ID: {FoodId}",
            command.FoodId);

        var request = command.Request;

        var food = await _foodRepository.GetByIdAsync(
            command.FoodId,
            cancellationToken);

        if (food is null)
        {
            return FoodErrors.NotFound;
        }

        bool duplicateName = await _foodRepository.ExistsWithTheGivenName(food.Name.ToLower(), cancellationToken);

        if (duplicateName)
            return FoodErrors.DuplicateName;

        UploadFileResponse? image = null;
        if (request.Image is not null)
        {
            image = await _fileService.UploadAsync(
                request.Image,
                cancellationToken);
        }

        var result = food.Update(
            request.Name,
            request.Description,
            request.Price,
            request.CategoryId,
            image?.Url,
            image?.PublicId,
            request.PreparationTimeMinutes,
            request.Calories);

        if (result.IsError)
        {
            return result.TopError;
        }

        await _foodRepository.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByTagAsync(
            $"food:{food.Id}",
            cancellationToken);

        await _cacheService.RemoveByTagAsync(
            $"restaurant:{food.RestaurantId}:menu",
            cancellationToken);

        await _cacheService.RemoveByTagAsync(
            "foods",
            cancellationToken);

        logger.LogInformation(
            "Successfully updated Food ID: {FoodId}",
            food.Id);

        return new UpdateFoodResponse(food.Id);
    }
}