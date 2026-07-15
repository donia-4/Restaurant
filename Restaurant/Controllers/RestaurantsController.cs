using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Features.Branches.Queries.GetRestaurantBranches;
using Restaurant.Application.Features.Categories.Queries.GetRestaurantCategories;
using Restaurant.Application.Features.Foods.Queries.GetRestaurantMenu;
using Restaurant.Application.Features.Restaurants.Commands.ChangeRestaurantAvailability;
using Restaurant.Application.Features.Restaurants.Commands.CreateRestaurant;
using Restaurant.Application.Features.Restaurants.Commands.ReviewRestaurant;
using Restaurant.Application.Features.Restaurants.Commands.UpdateRestaurant;
using Restaurant.Application.Features.Restaurants.CreateRestaurant;
using Restaurant.Application.Features.Restaurants.Dtos.ChangeRestaurantAvailability;
using Restaurant.Application.Features.Restaurants.Dtos.CreateRestaurant;
using Restaurant.Application.Features.Restaurants.Dtos.ReviewRestaurant;
using Restaurant.Application.Features.Restaurants.Dtos.SearchRestaurants;
using Restaurant.Application.Features.Restaurants.Dtos.UpdateRestaurant;
using Restaurant.Application.Features.Restaurants.Queries.GetAllRestaurants;
using Restaurant.Application.Features.Restaurants.Queries.SearchRestaurants;

namespace Restaurant.API.Controllers;

[Route("api/restaurants")]
public sealed class RestaurantsController(ISender sender)
    : ApiController
{
    [AllowAnonymous]
    [HttpPost("request")]
    public async Task<IActionResult> CreateRestaurant(
        [FromForm] CreateRestaurantRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateRestaurantCommand(request);

        var result = await sender.Send(
            command,
            cancellationToken);

        if (result.IsError)
        {
            return Problem(result.Errors);
        }

        return CreatedEnvelope(
            result.Value,
            "Restaurant request submitted successfully");
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllRestaurants(
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetAllRestaurantsQuery(),
            cancellationToken);
        return result.Match<IActionResult>(
            response => OkEnvelope(
                response,
                "Restaurants retrieved successfully"),
            Problem);
    }

    [HttpPatch("{restaurantId:guid}")]
    public async Task<IActionResult> UpdateRestaurant(
    Guid restaurantId,
    [FromForm] UpdateRestaurantRequest request,
    CancellationToken cancellationToken)
    {
        var command = new UpdateRestaurantCommand(
            restaurantId,
            request.Name,
            request.Description,
            request.Phone,
            request.Email,
            request.CuisineType,
            request.Address);

        var result = await sender.Send(
            command,
            cancellationToken);

        return result.Match<IActionResult>(
            response => OkEnvelope(
                response,
                "Restaurant updated successfully"),
            Problem);
    }

    [AllowAnonymous]
    [HttpGet("{restaurantId:guid}/branches")]
    public async Task<IActionResult> GetRestaurantBranches(
    Guid restaurantId,
    CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetRestaurantBranchesQuery(restaurantId),
            cancellationToken);

        return result.Match<IActionResult>(
            response => OkEnvelope(
                response,
                "Restaurant branches retrieved successfully"),
            Problem);
    }

    [AllowAnonymous]
    [HttpGet("{restaurantId:guid}/menu")]
    public async Task<IActionResult> GetRestaurantMenu(Guid restaurantId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetRestaurantMenuQuery(restaurantId), cancellationToken);
        return result.Match<IActionResult>(r => OkEnvelope(r, "Restaurant menu retrieved successfully"), Problem);
    }
    [AllowAnonymous]
    [HttpGet("{restaurantId:guid}/categories")]
    public async Task<IActionResult> GetRestaurantCategories(
    Guid restaurantId,
    CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetRestaurantCategoriesQuery(restaurantId),
            cancellationToken);

        return result.Match<IActionResult>(
            response => OkEnvelope(
                response,
                "Categories retrieved successfully"),
            Problem);
    }
    // ==========================================
    // REVIEW (Admin: Approve / Reject / Request Modification)
    // ==========================================
    [AllowAnonymous]
    [HttpPut("{restaurantId:guid}/review")]
    public async Task<IActionResult> ReviewRestaurant(
        Guid restaurantId,
        [FromForm] ReviewRestaurantRequest request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new ReviewRestaurantCommand(restaurantId, request),
            cancellationToken);

        return result.Match<IActionResult>(
            response => OkEnvelope(
                response,
                "Restaurant reviewed successfully"),
            Problem);
    }

    // ==========================================
    // CHANGE AVAILABILITY (Owner: Open / Closed / TemporarilyClosed / UnderMaintenance)
    // ==========================================
    [AllowAnonymous]
    [HttpPut("{restaurantId:guid}/availability")]
    public async Task<IActionResult> ChangeRestaurantAvailability(
        Guid restaurantId,
        [FromForm] ChangeRestaurantAvailabilityRequest request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new ChangeRestaurantAvailabilityCommand(restaurantId, request),
            cancellationToken);

        return result.Match<IActionResult>(
            response => OkEnvelope(
                response,
                "Restaurant availability changed successfully"),
            Problem);
    }
    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<IActionResult> SearchRestaurants(
        [FromQuery] string? name,
        [FromQuery] string? city,
        [FromQuery] Restaurant.Domain.Restaurants.Enums.CuisineType? cuisineType,
        [FromQuery] Guid? categoryId,
        [FromQuery] Restaurant.Domain.Restaurants.Enums.RestaurantStatus? status,
        [FromQuery] decimal? minRating,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var request = new SearchRestaurantsRequest
        {
            Name = name,
            City = city,
            CuisineType = cuisineType,
            CategoryId = categoryId,
            Status = status,
            MinRating = minRating,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await sender.Send(
            new SearchRestaurantsQuery(request),
            cancellationToken);

        return result.Match<IActionResult>(
            r => OkEnvelope(r, "Restaurants searched successfully"),
            Problem);
    }
}