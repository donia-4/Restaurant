using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.Foods.Dtos.GetRestaurantMenu
{
    public sealed record MenuFoodResponse(Guid Id, string Name, string Description, decimal Price, string? Image, int PreparationTimeMinutes, int? Calories, bool IsAvailable);

}
