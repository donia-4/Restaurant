using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Features.Branches.Dtos.WorkingHours;

namespace Restaurant.Application.Features.Branches.Dtos.GetRestaurantBranches
{
    public sealed record RestaurantBranchResponse(
        Guid Id,
        Guid RestaurantId,
        string Name,
        string Address,
        decimal Latitude,
        decimal Longitude,
        string Phone,
        bool IsActive,
        IReadOnlyCollection<WorkingHourResponse> WorkingHours);
}
