using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantsAPI.Migrations
{
    public partial class Mainenance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plants_Maintenance_MaintenanceId",
                table: "Plants");

            migrationBuilder.DropIndex(
                name: "IX_Plants_MaintenanceId",
                table: "Plants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Maintenance",
                table: "Maintenance");

            migrationBuilder.DropColumn(
                name: "MaintenanceId",
                table: "Plants");

            migrationBuilder.RenameTable(
                name: "Maintenance",
                newName: "Maintenances");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Maintenances",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "PlantId",
                table: "Maintenances",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Maintenances",
                table: "Maintenances",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_PlantId",
                table: "Maintenances",
                column: "PlantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_Plants_PlantId",
                table: "Maintenances",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_Plants_PlantId",
                table: "Maintenances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Maintenances",
                table: "Maintenances");

            migrationBuilder.DropIndex(
                name: "IX_Maintenances_PlantId",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "PlantId",
                table: "Maintenances");

            migrationBuilder.RenameTable(
                name: "Maintenances",
                newName: "Maintenance");

            migrationBuilder.AddColumn<Guid>(
                name: "MaintenanceId",
                table: "Plants",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Maintenance",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Maintenance",
                table: "Maintenance",
                column: "Id");

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
    }
}
