using MediatR;
using Restaurant.Domain.Restaurants.Enums;
using Restaurant.Domain.Results;
using Restaurant.Application.Features.Restaurants.Dtos.CreateRestaurant;

namespace Restaurant.Application.Features.Restaurants.Commands.CreateRestaurant;

public sealed record CreateRestaurantCommand(
    CreateRestaurantRequest Request)
    : IRequest<Result<CreateRestaurantResponse>>;