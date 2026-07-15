using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.DeliveryZones.Dtos.UpdateDeliveryZone;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.DeliveryZones.Command.UpdateDeliveryZone
{
    public sealed record UpdateDeliveryZoneCommand(
    Guid ZoneId,
    UpdateDeliveryZoneRequest Request
) : IRequest<Result<UpdateDeliveryZoneResponse>>;
}
