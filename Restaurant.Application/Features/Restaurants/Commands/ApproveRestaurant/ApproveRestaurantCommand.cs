using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.Restaurants.Dtos.ApproveRestaurant;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Commands.ApproveRestaurant
{
    public sealed record ApproveRestaurantCommand(Guid RestaurantId)
        : IRequest<Result<ApproveRestaurantResponse>>;
}
