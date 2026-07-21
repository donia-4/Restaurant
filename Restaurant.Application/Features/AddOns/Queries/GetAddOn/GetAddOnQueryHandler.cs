using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.AddOns.Dtos.GetAddOn;
using Restaurant.Domain.AddOns;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.AddOns.Queries.GetAddOn
{
    public sealed class GetAddOnQueryHandler(
    IAddOnRepository addOnRepository, ILogger<GetAddOnQueryHandler> logger)
    : IRequestHandler<GetAddOnQuery, Result<GetAddOnResponse>>
    {
        public async Task<Result<GetAddOnResponse>> Handle(
            GetAddOnQuery query,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Processing GetAddOnQuery for AddOn ID: {AddOnId}",
                query.AddOnId);

            var addOn = await addOnRepository.GetByIdAsync(
                query.AddOnId,
                cancellationToken);

            if (addOn is null)
                return AddOnErrors.NotFound;

            logger.LogInformation(
                "Successfully retrieved AddOn with ID: {AddOnId}",
                query.AddOnId);

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
