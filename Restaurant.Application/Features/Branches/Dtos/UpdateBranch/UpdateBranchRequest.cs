using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Features.Branches.Dtos.WorkingHours;

namespace Restaurant.Application.Features.Branches.Dtos.UpdateBranch
{
    public sealed record UpdateBranchRequest(
        string? Name,
        string? Address,
        decimal? Latitude,
        decimal? Longitude,
        string? Phone,
        IReadOnlyCollection<WorkingHourRequest>? WorkingHours);
}
