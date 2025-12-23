using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkladBlazorApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithOperationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "WarehouseOperations");

            migrationBuilder.AddColumn<int>(
                name: "OperationType",
                table: "WarehouseOperations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationType",
                table: "WarehouseOperations");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "WarehouseOperations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
