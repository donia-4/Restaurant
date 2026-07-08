using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Domain.Results;

namespace Restaurant.Domian.Foods
{
    public static class FoodErrors
    {
        public static readonly Error InvalidRestaurant = Error.Validation("Food.Restaurant.Invalid", "Restaurant is required.");

        public static readonly Error InvalidName = Error.Validation("Food.Name.Required", "Food name is required.");

        public static readonly Error InvalidPrice = Error.Validation("Food.Price.Invalid", "Food price must be greater than zero.");

        public static readonly Error NotFound = Error.NotFound("Food.NotFound", "Food was not found.");

        public static readonly Error InvalidCategory = Error.Validation("Food.Category.Required", "Food category is required.");
    }
}
