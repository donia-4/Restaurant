using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.AddOns.Dtos.GetFoodAddOns;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.AddOns.Queries.GetFoodAddOns
{
    public sealed record GetFoodAddOnsQuery(Guid FoodId) : IRequest<Result<IReadOnlyList<GetFoodAddOnsResponse>>>;

}
