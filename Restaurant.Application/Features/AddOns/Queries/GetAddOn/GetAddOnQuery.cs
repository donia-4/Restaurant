using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.AddOns.Dtos.GetAddOn;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.AddOns.Queries.GetAddOn
{
    public sealed record GetAddOnQuery(Guid AddOnId) : IRequest<Result<GetAddOnResponse>>;

}
