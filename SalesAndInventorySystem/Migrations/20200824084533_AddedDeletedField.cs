using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesAndInventorySystem.Migrations
{
    public partial class AddedDeletedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Items",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "ItemPrices",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ItemPrices");
        }
    }
}
