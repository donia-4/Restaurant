using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.Restaurants.Dtos.RequestRestaurantModification;
using Restaurant.Domain.Restaurants.Enums;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Restaurants.Commands.RequestRestaurantModification
{
    public sealed record RequestRestaurantModificationCommand(Guid RestaurantId)
        : IRequest<Result<RequestRestaurantModificationResponse>>;
}
