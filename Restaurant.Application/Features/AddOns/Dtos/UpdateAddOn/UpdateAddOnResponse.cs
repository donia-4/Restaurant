using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.AddOns.Dtos.UpdateAddOn
{
    public sealed record UpdateAddOnResponse(
        Guid Id,
        string Name,
        decimal Price,
        bool IsRequired,
        int? MaxQuantity
    );
}
