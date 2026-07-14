using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Restaurant.Domain.Restaurants.Enums;

namespace Restaurant.Application.Features.Restaurants.Commands.ChangeRestaurantAvailability
{
    public sealed class ChangeRestaurantAvailabilityCommandValidator : AbstractValidator<ChangeRestaurantAvailabilityCommand>
    {
        public ChangeRestaurantAvailabilityCommandValidator()
        {
            RuleFor(x => x.RestaurantId)
                .NotEmpty().WithMessage("Restaurant ID is required.");

            RuleFor(x => x.Request.Status)
                .Must(status => status is RestaurantStatus.Open
                             or RestaurantStatus.Closed
                             or RestaurantStatus.TemporarilyClosed
                             or RestaurantStatus.UnderMaintenance)
                .WithMessage("Status must be Open, Closed, TemporarilyClosed, or UnderMaintenance.");
        }
    }
}
