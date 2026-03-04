using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsAssistant.Migrations
{
    /// <inheritdoc />
    public partial class EditNotChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TruckDriverBreak",
                table: "Routes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TruckVmax",
                table: "Routes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TruckDriverBreak",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "TruckVmax",
                table: "Routes");
        }
    }
}
