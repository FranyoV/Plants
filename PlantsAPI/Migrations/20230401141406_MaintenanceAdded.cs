using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantsAPI.Migrations
{
    public partial class MaintenanceAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MaintenanceId",
                table: "Plants",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Maintenance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Interval = table.Column<int>(type: "int", nullable: false),
                    LastNotification = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextNotification = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintenance", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plants_MaintenanceId",
                table: "Plants",
                column: "MaintenanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plants_Maintenance_MaintenanceId",
                table: "Plants",
                column: "MaintenanceId",
                principalTable: "Maintenance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plants_Maintenance_MaintenanceId",
                table: "Plants");

            migrationBuilder.DropTable(
                name: "Maintenance");

            migrationBuilder.DropIndex(
                name: "IX_Plants_MaintenanceId",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "MaintenanceId",
                table: "Plants");
        }
    }
}
