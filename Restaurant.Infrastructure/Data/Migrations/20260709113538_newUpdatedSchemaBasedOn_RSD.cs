using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newUpdatedSchemaBasedOn_RSD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Available",
                table: "Foods",
                newName: "IsVisible");

            migrationBuilder.AddColumn<decimal>(
                name: "AverageRating",
                table: "Restaurants",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "Restaurants",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CuisineType",
                table: "Restaurants",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Restaurants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalReviews",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Calories",
                table: "Foods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Foods",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "PreparationTimeMinutes",
                table: "Foods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AddOns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FoodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MaxQuantity = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddOns_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RestaurantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RestaurantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantImages_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryZones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ZoneName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DeliveryFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MinimumOrder = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PolygonGeoJson = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    RestaurantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryZones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryZones_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryZones_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkingHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RestaurantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingHours_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkingHours_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_CuisineType",
                table: "Restaurants",
                column: "CuisineType");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_Name_Status_IsApproved",
                table: "Restaurants",
                columns: new[] { "Name", "Status", "IsApproved" });

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_Status",
                table: "Restaurants",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_IsAvailable",
                table: "Foods",
                column: "IsAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_IsVisible",
                table: "Foods",
                column: "IsVisible");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_RestaurantId_Name",
                table: "Foods",
                columns: new[] { "RestaurantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_RestaurantId_DisplayOrder",
                table: "Categories",
                columns: new[] { "RestaurantId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_AddOns_FoodId",
                table: "AddOns",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Latitude_Longitude",
                table: "Branches",
                columns: new[] { "Latitude", "Longitude" });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_RestaurantId",
                table: "Branches",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryZones_BranchId",
                table: "DeliveryZones",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryZones_RestaurantId",
                table: "DeliveryZones",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantImages_RestaurantId_ImageType",
                table: "RestaurantImages",
                columns: new[] { "RestaurantId", "ImageType" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHours_BranchId_DayOfWeek",
                table: "WorkingHours",
                columns: new[] { "BranchId", "DayOfWeek" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHours_RestaurantId",
                table: "WorkingHours",
                column: "RestaurantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddOns");

            migrationBuilder.DropTable(
                name: "DeliveryZones");

            migrationBuilder.DropTable(
                name: "RestaurantImages");

            migrationBuilder.DropTable(
                name: "WorkingHours");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_CuisineType",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_Name_Status_IsApproved",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_Status",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Foods_IsAvailable",
                table: "Foods");

            migrationBuilder.DropIndex(
                name: "IX_Foods_IsVisible",
                table: "Foods");

            migrationBuilder.DropIndex(
                name: "IX_Foods_RestaurantId_Name",
                table: "Foods");

            migrationBuilder.DropIndex(
                name: "IX_Categories_RestaurantId_DisplayOrder",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "CuisineType",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "TotalReviews",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Calories",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "PreparationTimeMinutes",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "IsVisible",
                table: "Foods",
                newName: "Available");
        }
    }
}
