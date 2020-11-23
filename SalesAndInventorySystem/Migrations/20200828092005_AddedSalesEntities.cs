using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesAndInventorySystem.Migrations
{
    public partial class AddedSalesEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SaleTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    TransactionId = table.Column<string>(maxLength: 100, nullable: true),
                    IsPosted = table.Column<bool>(nullable: false),
                    DatePosted = table.Column<DateTime>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    TotalAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleTransactionItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    SaleTransactionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleTransactionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleTransactionItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaleTransactionItems_SaleTransactions_SaleTransactionId",
                        column: x => x.SaleTransactionId,
                        principalTable: "SaleTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleTransactionItems_ItemId",
                table: "SaleTransactionItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleTransactionItems_SaleTransactionId",
                table: "SaleTransactionItems",
                column: "SaleTransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleTransactionItems");

            migrationBuilder.DropTable(
                name: "SaleTransactions");
        }
    }
}
