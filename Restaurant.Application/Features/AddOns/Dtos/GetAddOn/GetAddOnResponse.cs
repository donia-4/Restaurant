using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.AddOns.Dtos.GetAddOn
{
    public sealed record GetAddOnResponse(
        Guid Id,
        Guid FoodId,
        string Name,
        decimal Price,
        bool IsRequired,
        int? MaxQuantity
    );
}
