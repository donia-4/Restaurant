using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.Categories.Dtos.CreateCategory
{
    public sealed record CreateCategoryRequest(Guid RestaurantId, string Name, int DisplayOrder = 0);

}
