using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsAssistant.Migrations
{
    /// <inheritdoc />
    public partial class AddTruckRouteRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Routes_TruckId",
                table: "Routes",
                column: "TruckId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Trucks_TruckId",
                table: "Routes",
                column: "TruckId",
                principalTable: "Trucks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Trucks_TruckId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_TruckId",
                table: "Routes");
        }
    }
}
