using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.Foods.Dtos.GetFoodById
{
    public sealed record FoodDetailsResponse(Guid Id, Guid RestaurantId, Guid CategoryId, string Name, string Description, decimal Price, string? Image, int PreparationTimeMinutes, int? Calories, bool IsAvailable, bool IsVisible);

}
