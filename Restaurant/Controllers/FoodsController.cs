using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Features.Foods.Commands.ChangeFoodAvailability;
using Restaurant.Application.Features.Foods.Commands.CreateFood;
using Restaurant.Application.Features.Foods.Commands.DeleteFood;
using Restaurant.Application.Features.Foods.Commands.HideFood;
using Restaurant.Application.Features.Foods.Commands.ShowFood;
using Restaurant.Application.Features.Foods.Commands.UpdateFood;
using Restaurant.Application.Features.Foods.Dtos.CreateFood;
using Restaurant.Application.Features.Foods.Dtos.UpdateFood;
using Restaurant.Application.Features.Foods.Queries.GetFoodById;

namespace Restaurant.API.Controllers
{
    [Route("api/foods")]
    public sealed class FoodsController(ISender sender) : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateFood([FromBody] CreateFoodRequest request, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new CreateFoodCommand(request), cancellationToken);
            if (result.IsError) return Problem(result.Errors);
            return CreatedEnvelope(result.Value, "Food item created successfully");
        }

        [AllowAnonymous]
        [HttpGet("{foodId:guid}")]
        public async Task<IActionResult> GetFoodById(Guid foodId, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetFoodByIdQuery(foodId), cancellationToken);
            return result.Match<IActionResult>(r => OkEnvelope(r, "Food item retrieved successfully"), Problem);
        }

        [AllowAnonymous]
        [HttpPut("{foodId:guid}")]
        public async Task<IActionResult> UpdateFood(Guid foodId, [FromForm] UpdateFoodRequest request, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new UpdateFoodCommand(foodId, request), cancellationToken);
            return result.Match<IActionResult>(r => OkEnvelope(r, "Food item updated successfully"), Problem);
        }

        [AllowAnonymous]
        [HttpDelete("{foodId:guid}")]
        public async Task<IActionResult> DeleteFood(Guid foodId, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new DeleteFoodCommand(foodId), cancellationToken);
            return result.Match<IActionResult>(_ => OkEnvelope((object?)null, "Food item deleted successfully"), Problem);
        }

        [AllowAnonymous]
        [HttpPatch("{foodId:guid}/hide")]
        public async Task<IActionResult> HideFood(Guid foodId, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new HideFoodCommand(foodId), cancellationToken);
            return result.Match<IActionResult>(_ => OkEnvelope((object?)null, "Food item hidden successfully"), Problem);
        }

        [AllowAnonymous]
        [HttpPatch("{foodId:guid}/show")]
        public async Task<IActionResult> ShowFood(Guid foodId, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new ShowFoodCommand(foodId), cancellationToken);
            return result.Match<IActionResult>(_ => OkEnvelope((object?)null, "Food item shown successfully"), Problem);
        }

        [AllowAnonymous]
        [HttpPatch("{foodId:guid}/availability")]
        public async Task<IActionResult> ChangeAvailability(Guid foodId, [FromBody] bool isAvailable, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new ChangeFoodAvailabilityCommand(foodId, isAvailable), cancellationToken);
            return result.Match<IActionResult>(_ => OkEnvelope((object?)null, "Food availability changed successfully"), Problem);
        }
    }
}
