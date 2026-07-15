using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.DeliveryZones.Commands.CreateDeliveryZone;
using Restaurant.Application.Features.DeliveryZones.Dtos.CreateDeliveryZone;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.DeliveryZones.Command.CreateDeliveryZone;
public sealed record CreateDeliveryZoneCommand(
    CreateDeliveryZoneRequest Request
) : IRequest<Result<CreateDeliveryZoneResponse>>;