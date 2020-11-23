using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesAndInventorySystem.Migrations
{
    public partial class AddedSettingsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    AutoNumberDelimiter = table.Column<string>(maxLength: 1, nullable: true),
                    PreviousAutoNumberDelimiter = table.Column<string>(maxLength: 1, nullable: true),
                    AutoNumberItem = table.Column<string>(maxLength: 50, nullable: true),
                    AutoNumberItemReset = table.Column<bool>(nullable: false),
                    AutoNumberSourcing = table.Column<string>(maxLength: 50, nullable: true),
                    AutoNumberSourcingReset = table.Column<bool>(nullable: false),
                    AutoNumberSale = table.Column<string>(maxLength: 50, nullable: true),
                    AutoNumberSaleReset = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
