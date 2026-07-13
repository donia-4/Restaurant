using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Features.Branches.Dtos.DeactivateBranch
{
    public sealed record DeactivateBranchResponse(Guid BranchId);
}
