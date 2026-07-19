using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.Restaurants.Dtos.SearchRestaurants;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Queries.SearchRestaurants
{
    public sealed class SearchRestaurantsQueryHandler(
    IRestaurantRepository restaurantRepository, ILogger<SearchRestaurantsQueryHandler> logger)
    : IRequestHandler<SearchRestaurantsQuery, Result<PaginatedList<SearchRestaurantResponse>>>
    {
        public async Task<Result<PaginatedList<SearchRestaurantResponse>>> Handle(
            SearchRestaurantsQuery query,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Processing SearchRestaurantsQuery with parameters: {@Request}",
                query.Request);

            var request = query.Request;

            // Search() returns IQueryable<Restaurant>
            var restaurantsQuery = restaurantRepository.Search(
                request.Name,
                request.City,
                request.CuisineType,
                request.CategoryId,
                request.Status,
                request.MinRating);

            // Apply BR-02: Only approved restaurants
            restaurantsQuery = restaurantsQuery.Where(r => r.IsApproved);

            // FIXED: OrderBy BEFORE Select (projection)
            restaurantsQuery = restaurantsQuery.OrderBy(r => r.Name);

            // Project to response DTO AFTER OrderBy
            var projectedQuery = restaurantsQuery.Select(r => new SearchRestaurantResponse
            {
                Id = r.Id,
                Name = r.Name,
                Logo = r.Logo,
                CuisineType = r.CuisineType,
                Rating = r.AverageRating,
                Status = r.Status,
                City = r.Address
            });

            // Apply pagination (in DB)
            var paginatedResult = await PaginatedList<SearchRestaurantResponse>.CreateAsync(
                projectedQuery,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            logger.LogInformation(
                "SearchRestaurantsQuery processed successfully. TotalCount: {TotalCount}, PageNumber: {PageNumber}, PageSize: {PageSize}",
                paginatedResult.TotalCount,
                paginatedResult.PageNumber,
                paginatedResult.PageSize);

            return paginatedResult;
        }
    }
}