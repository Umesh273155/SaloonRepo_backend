using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonSphere.Infrastructure.Migrations
{
    public partial class FixServicesServiceIdAutoIncrement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Drop Appointments foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Services_ServiceId",
                table: "Appointments");

            // 2. Enable AUTO_INCREMENT
            migrationBuilder.Sql(@"
                ALTER TABLE `Services`
                MODIFY COLUMN `ServiceId` INT NOT NULL AUTO_INCREMENT;
            ");

            // 3. Recreate foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Services_ServiceId",
                table: "Appointments",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Services_ServiceId",
                table: "Appointments");

            migrationBuilder.Sql(@"
                ALTER TABLE `Services`
                MODIFY COLUMN `ServiceId` INT NOT NULL;
            ");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Services_ServiceId",
                table: "Appointments",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}