using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace SalonSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSalonNotificationPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalonNotificationPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SalonId = table.Column<int>(type: "int", nullable: false),
                    AppointmentNotifications = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PaymentNotifications = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    StockNotifications = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EmailNotifications = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalonNotificationPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalonNotificationPreferences_Salons_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Salons",
                        principalColumn: "SalonId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SalonNotificationPreferences_SalonId",
                table: "SalonNotificationPreferences",
                column: "SalonId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalonNotificationPreferences");
        }
    }
}
