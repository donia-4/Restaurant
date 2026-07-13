using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.Branches.Dtos.DeactivateBranch;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Branches.Commands.DeactivateBranch
{
    public sealed record DeactivateBranchCommand(Guid BranchId)
    : IRequest<Result<DeactivateBranchResponse>>;
}
