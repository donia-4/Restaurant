using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.AddOns.Dtos.UpdateAddOn
{
    /// <summary>
    /// Partial update request - all fields are optional.
    /// Only provided fields will be updated.
    /// </summary>
    public sealed record UpdateAddOnRequest(
        string? Name = null,
        decimal? Price = null,
        bool? IsRequired = null,
        int? MaxQuantity = null
    );
}
