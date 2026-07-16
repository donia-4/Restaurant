namespace Restaurant.Application.Features.Foods.Dtos.SearchFoods;

public sealed record SearchFoodResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string? Image,
    Guid CategoryId,
    string CategoryName,
    int PreparationTimeMinutes,
    int? Calories,
    bool IsAvailable,
    bool IsVisible);