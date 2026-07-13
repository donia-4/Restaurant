using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.Foods.Dtos.UpdateFood;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Commands.UpdateFood
{
    public sealed record UpdateFoodCommand(Guid FoodId, UpdateFoodRequest Request) : IRequest<Result<UpdateFoodResponse>>;

}
