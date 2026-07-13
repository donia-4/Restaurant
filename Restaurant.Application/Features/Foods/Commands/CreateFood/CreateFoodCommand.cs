using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.Foods.Dtos.CreateFood;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Commands.CreateFood
{
    public sealed record CreateFoodCommand(CreateFoodRequest Request) : IRequest<Result<CreateFoodResponse>>;

}
