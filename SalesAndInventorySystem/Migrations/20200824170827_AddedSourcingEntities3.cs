using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesAndInventorySystem.Migrations
{
    public partial class AddedSourcingEntities3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DatePosted",
                table: "SourcingTransactions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPosted",
                table: "SourcingTransactions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatePosted",
                table: "SourcingTransactions");

            migrationBuilder.DropColumn(
                name: "IsPosted",
                table: "SourcingTransactions");
        }
    }
}
