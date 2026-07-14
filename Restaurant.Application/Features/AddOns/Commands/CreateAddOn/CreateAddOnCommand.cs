using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.AddOns.Dtos.CreateAddOn;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.AddOns.Commands.CreateAddOn
{
    public sealed record CreateAddOnCommand(CreateAddOnRequest Request) 
        : IRequest<Result<CreateAddOnResponse>>;
}
