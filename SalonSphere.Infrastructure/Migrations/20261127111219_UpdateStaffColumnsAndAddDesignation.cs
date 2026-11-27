
using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814

namespace SalonSphere.Infrastructure.Migrations
{
    public partial class UpdateStaffColumnsAndAddDesignation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DesignationId",
                table: "Staff",
                type: "int",
                nullable: false,
                defaultValue: 3);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoiningDate",
                table: "Staff",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "Staff",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "Staff",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Designations",
                columns: table => new
                {
                    DesignationId = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation(
                            "MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),

                    DesignationName = table.Column<string>(
                        type: "varchar(100)",
                        maxLength: 100,
                        nullable: false),

                    IsActive = table.Column<bool>(
                        type: "tinyint(1)",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_Designations",
                        x => x.DesignationId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Designations",
                columns: new[]
                {
                    "DesignationId",
                    "DesignationName",
                    "IsActive"
                },
                values: new object[,]
                {
                    { 1, "Manager", true },
                    { 2, "Senior Beautician", true },
                    { 3, "Beautician", true },
                    { 4, "Hair Stylist", true },
                    { 5, "Makeup Artist", true },
                    { 6, "Nail Artist", true },
                    { 7, "Receptionist", true },
                    { 8, "Tattoo Artist", true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staff_DesignationId",
                table: "Staff",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_SalonId_Email",
                table: "Staff",
                columns: new[] { "SalonId", "Email" });

            migrationBuilder.CreateIndex(
                name: "IX_Staff_SalonId_Phone",
                table: "Staff",
                columns: new[] { "SalonId", "Phone" });

            migrationBuilder.CreateIndex(
                name: "IX_Designations_DesignationName",
                table: "Designations",
                column: "DesignationName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Designations_DesignationId",
                table: "Staff",
                column: "DesignationId",
                principalTable: "Designations",
                principalColumn: "DesignationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Designations_DesignationId",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Staff_DesignationId",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Staff_SalonId_Email",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Staff_SalonId_Phone",
                table: "Staff");

            migrationBuilder.DropTable(
                name: "Designations");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "JoiningDate",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Staff");
        }
    }
}
