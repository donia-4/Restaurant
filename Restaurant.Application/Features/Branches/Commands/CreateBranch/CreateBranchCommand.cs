using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Features.Branches.Dtos.CreateBranch;
using Restaurant.Application.Features.Branches.Dtos.WorkingHours;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Branches.Commands.CreateBranch
{
    public sealed record CreateBranchCommand(
     CreateBranchRequest Request)
     : IRequest<Result<CreateBranchResponse>>;
}
