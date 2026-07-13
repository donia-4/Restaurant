using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.Branches.Dtos.UpdateBranch;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Branches.Commands.UpdateBranch
{
    public sealed record UpdateBranchCommand(
        Guid BranchId,
        UpdateBranchRequest Request)
        : IRequest<Result<UpdateBranchResponse>>;
}
