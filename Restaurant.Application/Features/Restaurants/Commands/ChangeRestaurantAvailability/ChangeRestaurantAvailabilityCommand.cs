using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.Restaurants.Dtos.ChangeRestaurantAvailability;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Commands.ChangeRestaurantAvailability
{
    public sealed record ChangeRestaurantAvailabilityCommand(
        Guid RestaurantId,
        ChangeRestaurantAvailabilityRequest Request
    ) : IRequest<Result<ChangeRestaurantAvailabilityResponse>>;
}
