using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Dtos;
using Restaurant.Application.Features.Restaurants.Dtos.UpdateRestaurant;
using Restaurant.Domain.Restaurants.Enums;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Commands.UpdateRestaurant;

public sealed record UpdateRestaurantCommand(
    Guid RestaurantId,
    string? Name,
    string? Description,
    string? Phone,
    string? Email,
    CuisineType? CuisineType,
    string? Address,
    FileUpload? Logo,
    FileUpload? CoverImage)
    : IRequest<Result<UpdateRestaurantResponse>>;
