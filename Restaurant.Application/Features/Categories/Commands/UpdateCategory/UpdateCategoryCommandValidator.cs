using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Restaurant.Application.Features.Categories.Commands.UpdateCategory
{
    public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.Request.Name).NotEmpty().MaximumLength(100);
        }
    }

}
