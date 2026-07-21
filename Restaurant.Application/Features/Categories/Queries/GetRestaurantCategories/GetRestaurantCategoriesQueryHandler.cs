using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.Categories.Dtos.GetRestaurantCategories;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Categories.Queries.GetRestaurantCategories;

public sealed class GetRestaurantCategoriesQueryHandler(
    ICategoryRepository categoryRepository,
    ILogger<GetRestaurantCategoriesQueryHandler> logger)
    : IRequestHandler<GetRestaurantCategoriesQuery, Result<PaginatedList<CategoryResponse>>>
{
    public async Task<Result<PaginatedList<CategoryResponse>>> Handle(
        GetRestaurantCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling GetRestaurantCategoriesQuery for RestaurantId: {RestaurantId}, PageNumber: {PageNumber}, PageSize: {PageSize}",
            request.RestaurantId,
            request.PageNumber,
            request.PageSize);

        var query = categoryRepository
            .GetByRestaurantId(request.RestaurantId)
            .Select(category => new CategoryResponse(
                category.Id,
                category.RestaurantId,
                category.Name,
                category.DisplayOrder,
                category.Foods.Count));

        var paginatedCategories =
            await PaginatedList<CategoryResponse>.CreateAsync(
                query,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

        logger.LogInformation(
            "Retrieved {ReturnedCount} categories out of {TotalCount} for RestaurantId: {RestaurantId}",
            paginatedCategories.Items.Count,
            paginatedCategories.TotalCount,
            request.RestaurantId);

        return paginatedCategories;
    }
}