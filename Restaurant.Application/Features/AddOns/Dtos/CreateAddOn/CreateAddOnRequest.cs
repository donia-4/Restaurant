using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.AddOns.Dtos.CreateAddOn
{
    public sealed record CreateAddOnRequest(
        Guid FoodId,
        string Name,
        decimal Price,
        bool IsRequired,
        int? MaxQuantity);
}
