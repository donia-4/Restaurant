namespace Restaurant.Application.Features.Foods.Dtos.SearchFoods;

public sealed record SearchFoodsRequest(
    Guid RestaurantId,
    string? Name,
    string? CategoryName,
    decimal? MinPrice,
    decimal? MaxPrice,
    int PageNumber = 1,
    int PageSize = 10);