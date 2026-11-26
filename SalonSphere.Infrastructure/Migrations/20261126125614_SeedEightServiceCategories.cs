
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonSphere.Infrastructure.Migrations
{
    public partial class SeedEightServiceCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ServiceCategories",
                columns: new[] { "CategoryId", "CategoryName", "Description", "IsActive", "CreatedAt" },
                values: new object[,]
                {
                    { 1, "Hair", "Hair cutting, styling and hair treatments", true, new DateTime(2026, 11, 26) },
                    { 2, "Skin Care", "Facial, cleanup and skincare treatments", true, new DateTime(2026, 11, 26) },
                    { 3, "Nail Care", "Nail extensions, nail art and nail treatments", true, new DateTime(2026, 11, 26) },
                    { 4, "Tattoo", "Tattoo designing and tattoo services", true, new DateTime(2026, 11, 26) },
                    { 5, "Makeup", "Party makeup, bridal makeup and professional makeup", true, new DateTime(2026, 11, 26) },
                    { 6, "Waxing", "Facial and body waxing services", true, new DateTime(2026, 11, 26) },
                    { 7, "Threading", "Eyebrow, upper lip and facial threading", true, new DateTime(2026, 11, 26) },
                    { 8, "Spa & Massage", "Hair spa, body massage and relaxation treatments", true, new DateTime(2026, 11, 26) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8 });
        }
    }
}
