using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.Foods.Dtos.GetFoodById;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Queries.GetFoodById
{
    public sealed record GetFoodByIdQuery(Guid FoodId) : IRequest<Result<FoodDetailsResponse>>;

}
