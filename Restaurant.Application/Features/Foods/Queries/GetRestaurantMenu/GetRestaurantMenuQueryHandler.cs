using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.Foods.Dtos.GetRestaurantMenu;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Queries.GetRestaurantMenu;

public sealed class GetRestaurantMenuQueryHandler(
    ICategoryRepository categoryRepository,
    ILogger<GetRestaurantMenuQueryHandler> logger)
    : IRequestHandler<GetRestaurantMenuQuery, Result<PaginatedList<MenuCategoryResponse>>>
{
    public async Task<Result<PaginatedList<MenuCategoryResponse>>> Handle(
        GetRestaurantMenuQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling GetRestaurantMenuQuery for RestaurantId: {RestaurantId}, PageNumber: {PageNumber}, PageSize: {PageSize}",
            request.RestaurantId,
            request.PageNumber,
            request.PageSize);

        var categories = await categoryRepository.GetByRestaurantIdAsync(
            request.RestaurantId,
            cancellationToken);

        var menu = categories
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new MenuCategoryResponse(
                c.Id,
                c.Name,
                c.DisplayOrder,
                c.Foods
                    .Where(f => f.IsVisible)
                    .Select(f => new MenuFoodResponse(
                        f.Id,
                        f.Name,
                        f.Description,
                        f.Price,
                        f.Image,
                        f.PreparationTimeMinutes,
                        f.Calories,
                        f.IsAvailable))
                    .ToList()))
            .ToList();

        var paginatedResult = new PaginatedList<MenuCategoryResponse>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = menu.Count,
            TotalPages = (int)Math.Ceiling(menu.Count / (double)request.PageSize),
            Items = menu
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList()
        };

        logger.LogInformation(
            "Retrieved {ReturnedCount} menu categories out of {TotalCount} for RestaurantId: {RestaurantId}",
            paginatedResult.Items.Count,
            paginatedResult.TotalCount,
            request.RestaurantId);

        return paginatedResult;
    }
}