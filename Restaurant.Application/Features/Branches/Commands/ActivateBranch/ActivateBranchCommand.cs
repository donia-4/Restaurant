using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.Branches.Dtos.ActivateBranch;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Branches.Commands.ActivateBranch
{
    public sealed record ActivateBranchCommand(Guid BranchId)
    : IRequest<Result<ActivateBranchResponse>>;
}
