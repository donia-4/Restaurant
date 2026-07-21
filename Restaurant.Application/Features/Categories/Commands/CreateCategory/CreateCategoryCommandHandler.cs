using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common.Interfaces.Repositories;
using Restaurant.Application.Features.Categories.Dtos.CreateCategory;
using Restaurant.Domain.Categories;
using Restaurant.Domain.Foods;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Categories.Commands.CreateCategory
{
    public sealed class CreateCategoryCommandHandler(IRestaurantRepository restaurantRepository,
        ICategoryRepository categoryRepository, ILogger<CreateCategoryCommandHandler> logger) 
        : IRequestHandler<CreateCategoryCommand, Result<CreateCategoryResponse>>
    {
        public async Task<Result<CreateCategoryResponse>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling CreateCategoryCommand for RestaurantId: {RestaurantId}, Name: {Name}", command.Request.RestaurantId, command.Request.Name);

            var request = command.Request;
            
            bool duplicateName = await categoryRepository.ExistsWithTheGivenName(request.Name.ToLower(), cancellationToken);
            if (duplicateName) return CategoryErrors.DuplicateName;

            var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId, cancellationToken);
            if (restaurant is null) return Restaurant.Domain.Restaurants.RestaurantErrors.NotFound;

            var categoryResult = Category.Create(Guid.NewGuid(), request.RestaurantId, request.Name, request.DisplayOrder);
            if (categoryResult.IsError) return categoryResult.TopError;

            var addResult = restaurant.AddCategory(categoryResult.Value);
            if (addResult.IsError) return addResult.TopError;

            await categoryRepository.AddAsync(categoryResult.Value, cancellationToken);
            await categoryRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Category created successfully with Id: {CategoryId} for RestaurantId: {RestaurantId}", categoryResult.Value.Id, request.RestaurantId);

            return new CreateCategoryResponse(categoryResult.Value.Id);
        }
    }
}
