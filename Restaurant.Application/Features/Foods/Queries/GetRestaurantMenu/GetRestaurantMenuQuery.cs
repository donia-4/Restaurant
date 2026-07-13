using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Features.Foods.Dtos.GetRestaurantMenu;
using Restaurant.Domain.Results;

namespace Restaurant.Application.Features.Foods.Queries.GetRestaurantMenu
{
    public sealed record GetRestaurantMenuQuery(Guid RestaurantId) : ICachedQuery<Result<IReadOnlyList<MenuCategoryResponse>>>
    {
        public string CacheKey => $"restaurant:{RestaurantId}:menu";
        public string[] Tags => [$"restaurant:{RestaurantId}:menu", "menu"];
        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}
