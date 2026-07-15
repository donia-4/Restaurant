using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Restaurant.Application.Features.DeliveryZones.Command.CreateDeliveryZone
{
    public sealed class CreateDeliveryZoneCommandValidator : AbstractValidator<CreateDeliveryZoneCommand>
    {
        public CreateDeliveryZoneCommandValidator()
        {
            RuleFor(x => x.Request.BranchId)
                .NotEmpty().WithMessage("Branch ID is required.");

            RuleFor(x => x.Request.ZoneName)
                .NotEmpty().WithMessage("Zone name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Request.DeliveryFee)
                .GreaterThanOrEqualTo(0).WithMessage("Delivery fee cannot be negative.");

            RuleFor(x => x.Request.MinimumOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum order cannot be negative.");
        }
    }

}
