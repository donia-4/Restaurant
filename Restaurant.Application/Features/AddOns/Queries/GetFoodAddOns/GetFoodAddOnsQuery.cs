using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Models;
using Restaurant.Application.Features.AddOns.Dtos.GetFoodAddOns;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.AddOns.Queries.GetFoodAddOns
{
    public sealed record GetFoodAddOnsQuery(
        Guid FoodId,
        int PageNumber = 1,
        int PageSize = 10)
        : IRequest<Result<PaginatedList<GetFoodAddOnsResponse>>>;
}
