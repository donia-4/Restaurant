using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.Categories.Dtos.ReorderCategories;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Categories.Commands.ReorderCategories
{
    public sealed record ReorderCategoriesCommand(IReadOnlyCollection<ReorderCategoryRequest> Items) : IRequest<Result<Updated>>;

}
