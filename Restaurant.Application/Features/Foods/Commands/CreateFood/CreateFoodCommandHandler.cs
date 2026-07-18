using MediatR;
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
    IFileService fileService)
    : IRequestHandler<CreateFoodCommand, Result<CreateFoodResponse>>
{
    public async Task<Result<CreateFoodResponse>> Handle(
        CreateFoodCommand command,
        CancellationToken cancellationToken)
    {
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

        return new CreateFoodResponse(foodResult.Value.Id);
    }
}