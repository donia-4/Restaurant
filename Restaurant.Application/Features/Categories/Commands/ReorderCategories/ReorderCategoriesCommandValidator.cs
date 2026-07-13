using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Restaurant.Application.Features.Categories.Commands.ReorderCategories
{
    public sealed class ReorderCategoriesCommandValidator : AbstractValidator<ReorderCategoriesCommand>
    {
        public ReorderCategoriesCommandValidator()
        {
            RuleFor(x => x.Items).NotEmpty();
            RuleForEach(x => x.Items).ChildRules(item => item.RuleFor(x => x.CategoryId).NotEmpty());
        }
    }
}
