using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Features.Branches.Queries.GetRestaurantBranches;
using Restaurant.Application.Features.Foods.Queries.GetRestaurantMenu;
using Restaurant.Application.Features.Restaurants.Commands.ApproveRestaurant;
using Restaurant.Application.Features.Restaurants.Commands.CreateRestaurant;
using Restaurant.Application.Features.Restaurants.Commands.RejectRestaurant;
using Restaurant.Application.Features.Restaurants.Commands.RequestRestaurantModification;
using Restaurant.Application.Features.Restaurants.Commands.UpdateRestaurant;
using Restaurant.Application.Features.Restaurants.CreateRestaurant;
using Restaurant.Application.Features.Restaurants.Dtos.CreateRestaurant;
using Restaurant.Application.Features.Restaurants.Dtos.UpdateRestaurant;
using Restaurant.Application.Features.Restaurants.Queries.GetAllRestaurants;

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
    [HttpPut("{id:guid}/approve")]
    public async Task<IActionResult> Approve(
    Guid id,
    CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new ApproveRestaurantCommand(id),
            cancellationToken);

        return result.Match<IActionResult>(
            response => OkEnvelope(
                response,
                "Restaurant approved successfully"),
            Problem);
    }

    [AllowAnonymous]
    [HttpPut("{id:guid}/reject")]
    public async Task<IActionResult> Reject(
        Guid id,
        [FromBody] string? reason,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RejectRestaurantCommand(id, reason),
            cancellationToken);

        return result.Match<IActionResult>(
            response => OkEnvelope(
                response,
                "Restaurant rejected successfully"),
            Problem);
    }

    [AllowAnonymous]
    [HttpPut("{id:guid}/request-modification")]
    public async Task<IActionResult> RequestModification(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RequestRestaurantModificationCommand(id),
            cancellationToken);

        return result.Match<IActionResult>(
            response => OkEnvelope(
                response,
                "Restaurant modification requested successfully"),
            Problem);
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
}