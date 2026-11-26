using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateServicesTable : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation(
                            "MySql:ValueGenerationStrategy",
                            MySql.EntityFrameworkCore.Metadata.MySQLValueGenerationStrategy.IdentityColumn),

                    SalonId = table.Column<int>(type: "int", nullable: false),

                    ServiceName = table.Column<string>(
                        type: "varchar(150)",
                        maxLength: 150,
                        nullable: false),

                    Description = table.Column<string>(
                        type: "varchar(500)",
                        maxLength: 500,
                        nullable: true),

                    CategoryId = table.Column<int>(type: "int", nullable: false),

                    DurationMinutes = table.Column<int>(type: "int", nullable: false),

                    Price = table.Column<decimal>(
                        type: "decimal(10,2)",
                        precision: 10,
                        scale: 2,
                        nullable: false),

                    IsActive = table.Column<bool>(
                        type: "tinyint(1)",
                        nullable: false,
                        defaultValue: true),

                    CreatedAt = table.Column<DateTime>(
                        type: "datetime(6)",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ServiceId);

                    table.ForeignKey(
                        name: "FK_Services_Salons_SalonId",
                        column: x => x.SalonId,
                        principalTable: "salons",
                        principalColumn: "SalonId",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_Services_ServiceCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "servicecategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_CategoryId",
                table: "Services",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_SalonId_IsActive",
                table: "Services",
                columns: new[] { "SalonId", "IsActive" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Services");
        }

    }
}
