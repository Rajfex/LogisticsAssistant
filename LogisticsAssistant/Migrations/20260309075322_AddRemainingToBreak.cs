using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsAssistant.Migrations
{
    /// <inheritdoc />
    public partial class AddRemainingToBreak : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RemainingToBreak",
                table: "Routes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingToBreak",
                table: "Routes");
        }
    }
}
