using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Features.Restaurants.Commands.CreateRestaurant;
using Restaurant.Application.Features.Restaurants.CreateRestaurant;
using Restaurant.Application.Features.Restaurants.Dtos.CreateRestaurant;

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
}