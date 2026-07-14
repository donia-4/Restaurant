using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.Restaurants.Dtos.ReviewRestaurant;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Commands.ReviewRestaurant
{
    public sealed record ReviewRestaurantCommand(
        Guid RestaurantId,
        ReviewRestaurantRequest Request
    ) : IRequest<Result<ReviewRestaurantResponse>>;

}
