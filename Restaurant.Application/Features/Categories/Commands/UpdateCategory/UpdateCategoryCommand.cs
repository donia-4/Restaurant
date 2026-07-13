using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.Categories.Dtos.UpdateCategory;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Categories.Commands.UpdateCategory
{
    public sealed record UpdateCategoryCommand(Guid CategoryId, UpdateCategoryRequest Request) : IRequest<Result<UpdateCategoryResponse>>;
}
