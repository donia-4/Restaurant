using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.Foods.Dtos.UpdateFood
{
    public sealed record UpdateFoodRequest(string Name, string Description, decimal Price, Guid CategoryId, string? Image, int PreparationTimeMinutes, int? Calories);

}
