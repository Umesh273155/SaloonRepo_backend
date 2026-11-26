using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SalonSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTattooServiceCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 25);

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 3,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Nail Care", "Nail extensions, nail art and nail treatments" });

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 4,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Tattoo", "Tattoo designing and tattoo services" });

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 5,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Makeup", "Party makeup, bridal makeup and professional makeup" });

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 6,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Waxing", "Facial and body waxing services" });

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 7,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Threading", "Eyebrow, upper lip and facial threading" });

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 8,
                column: "Description",
                value: "Hair spa, body massage and relaxation treatments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 3,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Makeup", "Party and bridal makeup services" });

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 4,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Nail Care", "Manicure, pedicure and nail extensions" });

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 5,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Waxing", "Body waxing and hair removal" });

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 6,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Threading", "Eyebrow and facial threading" });

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 7,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Bridal", "Bridal and pre-bridal services" });

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "CategoryId",
                keyValue: 8,
                column: "Description",
                value: "Spa and relaxation treatments");

            migrationBuilder.InsertData(
                table: "ServiceCategories",
                columns: new[] { "CategoryId", "CategoryName", "CreatedAt", "Description", "IsActive" },
                values: new object[,]
                {
                    { 9, "Tattoo", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tattoo designing, body art and tattoo services", true },
                    { 10, "Mehndi", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bridal and traditional mehndi designs", true },
                    { 11, "Eyelash & Eyebrow", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Eyelash extensions, lash lifts and eyebrow treatments", true },
                    { 12, "Hair Extensions", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hair extensions and hair weaving services", true },
                    { 13, "Hair Treatments", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Keratin, smoothening and hair botox treatments", true },
                    { 14, "Body Care", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Body polishing, scrubbing and body treatments", true },
                    { 15, "Bleach & D-Tan", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Face and body bleach and D-Tan services", true },
                    { 16, "Manicure & Pedicure", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hand and foot beauty treatments", true },
                    { 17, "Men's Grooming", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beard styling, shaving and men's grooming", true },
                    { 18, "Kids Salon", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Haircuts and grooming services for kids", true },
                    { 19, "Piercing", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ear and body piercing services", true },
                    { 20, "Permanent Makeup", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Microblading and permanent makeup services", true },
                    { 21, "Laser Treatments", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laser hair removal and skin treatments", true },
                    { 22, "Bridal Packages", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Complete bridal beauty packages", true },
                    { 23, "Hair Coloring", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Global hair color, highlights and root touch-up", true },
                    { 24, "Skin Treatments", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Advanced skincare and skin rejuvenation", true },
                    { 25, "Nail Art", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Creative nail designs and nail styling", true }
                });
        }
    }
}
