using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Features.Categories.Dtos.GetRestaurantCategories;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Categories.Queries.GetRestaurantCategories
{
    public sealed record GetRestaurantCategoriesQuery(Guid RestaurantId) : ICachedQuery<Result<IReadOnlyList<CategoryResponse>>>
    {
        public string CacheKey => $"restaurant:{RestaurantId}:categories";
        public string[] Tags => [$"restaurant:{RestaurantId}:categories", "categories"];
        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}
