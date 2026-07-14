using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.AddOns.Dtos.GetFoodAddOns;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.AddOns.Queries.GetFoodAddOns
{
    public sealed class GetFoodAddOnsQueryHandler(
    IFoodRepository foodRepository)
    : IRequestHandler<GetFoodAddOnsQuery, Result<IReadOnlyList<GetFoodAddOnsResponse>>>
    {
        public async Task<Result<IReadOnlyList<GetFoodAddOnsResponse>>> Handle(
            GetFoodAddOnsQuery query,
            CancellationToken cancellationToken)
        {
            var food = await foodRepository.GetByIdWithAddOnsAsync(
                query.FoodId,
                cancellationToken);

            if (food is null)
                return FoodErrors.NotFound;

            var response = food.AddOns
                .Select(a => new GetFoodAddOnsResponse(
                    a.Id,
                    a.Name,
                    a.Price,
                    a.IsRequired,
                    a.MaxQuantity))
                .ToList();

            return response;
        }
    }
}
