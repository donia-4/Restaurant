using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Restaurant.Application.Features.DeliveryZones.Command.UpdateDeliveryZone
{
    public sealed class UpdateDeliveryZoneCommandValidator : AbstractValidator<UpdateDeliveryZoneCommand>
    {
        public UpdateDeliveryZoneCommandValidator()
        {
            RuleFor(x => x.ZoneId).NotEmpty();

            When(x => x.Request.ZoneName is not null, () =>
            {
                RuleFor(x => x.Request.ZoneName).NotEmpty();
            });

            When(x => x.Request.DeliveryFee.HasValue, () =>
            {
                RuleFor(x => x.Request.DeliveryFee!.Value).GreaterThanOrEqualTo(0);
            });

            When(x => x.Request.MinimumOrder.HasValue, () =>
            {
                RuleFor(x => x.Request.MinimumOrder!.Value).GreaterThanOrEqualTo(0);
            });
        }
    }
}
