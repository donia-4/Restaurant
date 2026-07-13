using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Branches.Commands.DeleteBranch
{
    public sealed record DeleteBranchCommand(Guid BranchId)
    : IRequest<Result<Deleted>>;
}
