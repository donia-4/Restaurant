using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.Categories.Dtos.GetRestaurantCategories
{
    public sealed record FoodSummaryResponse(Guid Id, string Name, decimal Price, bool IsAvailable);

}
