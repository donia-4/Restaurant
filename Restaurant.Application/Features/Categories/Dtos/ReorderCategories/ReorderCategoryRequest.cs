using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.Categories.Dtos.ReorderCategories
{
    public sealed record ReorderCategoryRequest(Guid CategoryId, int DisplayOrder);

}
