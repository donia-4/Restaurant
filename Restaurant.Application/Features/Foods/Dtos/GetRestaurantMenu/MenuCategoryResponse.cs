using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.Foods.Dtos.GetRestaurantMenu
{
    public sealed record MenuCategoryResponse(Guid Id, string Name, int DisplayOrder, IReadOnlyCollection<MenuFoodResponse> Foods);

}
