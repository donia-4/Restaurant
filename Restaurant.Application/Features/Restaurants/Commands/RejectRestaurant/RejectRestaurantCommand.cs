using MediatR;
using Restaurant.Application.Features.Restaurants.Dtos.RejectRestaurant;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Commands.RejectRestaurant
{
    public sealed record RejectRestaurantCommand(
    Guid RestaurantId,
    string? Reason)
    : IRequest<Result<RejectRestaurantResponse>>;
}
