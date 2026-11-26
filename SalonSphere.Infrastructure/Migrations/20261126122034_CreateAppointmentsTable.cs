using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateAppointmentsTable : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation(
                            "MySql:ValueGenerationStrategy",
                            MySql.EntityFrameworkCore.Metadata.MySQLValueGenerationStrategy.IdentityColumn),

                    SalonId = table.Column<int>(type: "int", nullable: false),

                    CustomerName = table.Column<string>(
                        type: "varchar(150)", maxLength: 150, nullable: false),

                    CustomerPhone = table.Column<string>(
                        type: "varchar(15)", maxLength: 15, nullable: false),

                    ServiceId = table.Column<int>(type: "int", nullable: false),

                    StaffId = table.Column<int>(type: "int", nullable: false),

                    AppointmentDate = table.Column<DateTime>(
                        type: "datetime(6)", nullable: false),

                    AppointmentTime = table.Column<TimeSpan>(
                        type: "time(6)", nullable: false),

                    Amount = table.Column<decimal>(
                        type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),

                    Status = table.Column<string>(
                        type: "varchar(30)",
                        maxLength: 30,
                        nullable: false,
                        defaultValue: "Pending"),

                    Notes = table.Column<string>(
                        type: "varchar(500)", maxLength: 500, nullable: true),

                    CreatedAt = table.Column<DateTime>(
                        type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentId);

                    table.ForeignKey(
                        name: "FK_Appointments_Salons_SalonId",
                        column: x => x.SalonId,
                        principalTable: "salons",
                        principalColumn: "SalonId",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_Appointments_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_Appointments_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ServiceId",
                table: "Appointments",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_SalonId_AppointmentDate",
                table: "Appointments",
                columns: new[] { "SalonId", "AppointmentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_StaffId_AppointmentDate_AppointmentTime",
                table: "Appointments",
                columns: new[] { "StaffId", "AppointmentDate", "AppointmentTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Appointments");
        }

    }
}
