using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.AddOns.Dtos.GetAddOn;
using Restaurant.Domain.AddOns;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.AddOns.Queries.GetAddOn
{
    public sealed class GetAddOnQueryHandler(
    IAddOnRepository addOnRepository)
    : IRequestHandler<GetAddOnQuery, Result<GetAddOnResponse>>
    {
        public async Task<Result<GetAddOnResponse>> Handle(
            GetAddOnQuery query,
            CancellationToken cancellationToken)
        {
            var addOn = await addOnRepository.GetByIdAsync(
                query.AddOnId,
                cancellationToken);

            if (addOn is null)
                return AddOnErrors.NotFound;

            return new GetAddOnResponse(
                addOn.Id,
                addOn.FoodId,
                addOn.Name,
                addOn.Price,
                addOn.IsRequired,
                addOn.MaxQuantity);
        }
    }
}
