using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Common.Dtos;

namespace Restaurant.Application.Features.Foods.Dtos.UpdateFood
{
    public sealed record UpdateFoodRequest
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public decimal? Price { get; init; }
        public Guid? CategoryId { get; init; }
        public FileUpload? Image { get; init; }
        public int? PreparationTimeMinutes { get; init; }
        public int? Calories { get; init; }
    }
}
