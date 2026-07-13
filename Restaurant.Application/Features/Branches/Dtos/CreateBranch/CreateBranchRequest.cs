using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Features.Branches.Dtos.WorkingHours;


namespace Restaurant.Application.Features.Branches.Dtos.CreateBranch;

public sealed record CreateBranchRequest(
        Guid RestaurantId,
        string Name,
        string Address,
        decimal Latitude,
        decimal Longitude,
        string Phone,
        IReadOnlyCollection<WorkingHourRequest> WorkingHours);