using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.Categories.Dtos.CreateCategory;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Categories.Commands.CreateCategory
{
    public sealed record CreateCategoryCommand(CreateCategoryRequest Request) : IRequest<Result<CreateCategoryResponse>>;

}
