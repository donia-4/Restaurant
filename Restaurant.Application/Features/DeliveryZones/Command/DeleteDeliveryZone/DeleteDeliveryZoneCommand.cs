using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.DeliveryZones.Command.DeleteDeliveryZone
{
    public sealed record DeleteDeliveryZoneCommand(Guid ZoneId) : IRequest<Result<Deleted>>;

}
