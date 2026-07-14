using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.AddOns.Dtos.UpdateAddOn;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.AddOns.Commands.UpdateAddOn
{
    public sealed record UpdateAddOnCommand(Guid AddOnId,
        UpdateAddOnRequest Request) : IRequest<Result<UpdateAddOnResponse>>;
}
