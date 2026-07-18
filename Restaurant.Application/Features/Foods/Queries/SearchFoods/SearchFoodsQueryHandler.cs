using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.Foods.Dtos.SearchFoods;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Queries.SearchFoods;

public sealed class SearchFoodsQueryHandler(IFoodRepository foodRepository)
    : IRequestHandler<SearchFoodsQuery, Result<PaginatedList<SearchFoodResponse>>>
{
    public async Task<Result<PaginatedList<SearchFoodResponse>>> Handle(
        SearchFoodsQuery request,
        CancellationToken cancellationToken)
    {
        var paginatedFoods = await foodRepository.SearchAsync(
            request.RestaurantId,
            request.Name,
            request.CategoryName,
            request.MinPrice,
            request.MaxPrice,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var mappedItems = paginatedFoods.Items
            .Select(food => new SearchFoodResponse(
                food.Id,
                food.Name,
                food.Description,
                food.Price,
                food.Image,
                food.CategoryId,
                food.Category?.Name ?? string.Empty,
                food.PreparationTimeMinutes,
                food.Calories,
                food.IsAvailable,
                food.IsVisible))
            .ToList();

        var response = new PaginatedList<SearchFoodResponse>
        {
            PageNumber = paginatedFoods.PageNumber,
            PageSize = paginatedFoods.PageSize,
            TotalCount = paginatedFoods.TotalCount,
            TotalPages = paginatedFoods.TotalPages,
            Items = mappedItems
        };

        return response;
    }
}