using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesAndInventorySystem.Migrations
{
    public partial class AddedSourcingEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SourcingTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DateTime = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(maxLength: 200, nullable: true),
                    TotalAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourcingTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SourcingTransactionItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    PurchasePrice = table.Column<double>(nullable: false),
                    SourcingTransactionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourcingTransactionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourcingTransactionItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SourcingTransactionItems_SourcingTransactions_SourcingTransactionId",
                        column: x => x.SourcingTransactionId,
                        principalTable: "SourcingTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SourcingTransactionItems_ItemId",
                table: "SourcingTransactionItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SourcingTransactionItems_SourcingTransactionId",
                table: "SourcingTransactionItems",
                column: "SourcingTransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SourcingTransactionItems");

            migrationBuilder.DropTable(
                name: "SourcingTransactions");
        }
    }
}
