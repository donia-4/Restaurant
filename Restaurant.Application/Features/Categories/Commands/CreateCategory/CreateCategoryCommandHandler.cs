using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.Categories.Dtos.CreateCategory;
using Restaurant.Domain.Categories;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Categories.Commands.CreateCategory
{
    public sealed class CreateCategoryCommandHandler(IRestaurantRepository restaurantRepository, ICategoryRepository categoryRepository) : IRequestHandler<CreateCategoryCommand, Result<CreateCategoryResponse>>
    {
        public async Task<Result<CreateCategoryResponse>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId, cancellationToken);
            if (restaurant is null) return Restaurant.Domain.Restaurants.RestaurantErrors.NotFound;

            var categoryResult = Category.Create(Guid.NewGuid(), request.RestaurantId, request.Name, request.DisplayOrder);
            if (categoryResult.IsError) return categoryResult.TopError;

            var addResult = restaurant.AddCategory(categoryResult.Value);
            if (addResult.IsError) return addResult.TopError;

            await categoryRepository.AddAsync(categoryResult.Value, cancellationToken);
            await categoryRepository.SaveChangesAsync(cancellationToken);
            return new CreateCategoryResponse(categoryResult.Value.Id);
        }
    }
}
