using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class consideringsoftdeleteinreviewindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_RestaurantId_UserId",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RestaurantId_UserId_IsDeleted",
                table: "Reviews",
                columns: new[] { "RestaurantId", "UserId", "IsDeleted" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_RestaurantId_UserId_IsDeleted",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RestaurantId_UserId",
                table: "Reviews",
                columns: new[] { "RestaurantId", "UserId" },
                unique: true);
        }
    }
}
