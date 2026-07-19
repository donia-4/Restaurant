using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Dtos;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Interfaces.Services;
using Restaurant.Application.Features.Foods.Commands.CreateFood;
using Restaurant.Application.Features.Foods.Dtos.CreateFood;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Commands.CreateFood;

public sealed class CreateFoodCommandHandler(
    IRestaurantRepository restaurantRepository,
    ICategoryRepository categoryRepository,
    IFoodRepository foodRepository,
    IFileService fileService,
    ILogger<CreateFoodCommandHandler> logger)
    : IRequestHandler<CreateFoodCommand, Result<CreateFoodResponse>>
{
    public async Task<Result<CreateFoodResponse>> Handle(
        CreateFoodCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Processing CreateFoodCommand for Category ID: {CategoryId}",
            command.Request.CategoryId);

        var request = command.Request;

        var restaurant = await restaurantRepository.GetByIdAsync(
            request.RestaurantId,
            cancellationToken);

        if (restaurant is null)
        {
            return Restaurant.Domain.Restaurants.RestaurantErrors.NotFound;
        }

        var category = await categoryRepository.GetByIdAsync(
            request.CategoryId,
            cancellationToken);

        if (category is null)
        {
            return Restaurant.Domain.Categories.CategoryErrors.NotFound;
        }

        bool duplicateName = await foodRepository.ExistsWithTheGivenName(request.Name.ToLower(), cancellationToken);

        if (duplicateName)
            return FoodErrors.DuplicateName;

        UploadFileResponse? image = null;
        if (request.Image is not null)
        {
            image = await fileService.UploadAsync(
                request.Image,
                cancellationToken);
        }

        var foodResult = Food.Create(
            Guid.NewGuid(),
            request.RestaurantId,
            request.CategoryId,
            request.Name,
            request.Description,
            request.Price,
            image?.Url,
            image?.PublicId,
            request.PreparationTimeMinutes,
            request.Calories);

        if (foodResult.IsError)
        {
            return foodResult.TopError;
        }

        var addResult = restaurant.AddFood(foodResult.Value);
        if (addResult.IsError)
        {
            return addResult.TopError;
        }

        await foodRepository.AddAsync(
            foodResult.Value,
            cancellationToken);

        await foodRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Successfully created Food with ID: {FoodId} for Category ID: {CategoryId}",
            foodResult.Value.Id,
            command.Request.CategoryId);

        return new CreateFoodResponse(foodResult.Value.Id);
    }
}