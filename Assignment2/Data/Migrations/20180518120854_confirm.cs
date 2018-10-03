using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Assignment2.Data.Migrations
{
    public partial class confirm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    StoreID = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => new { x.OrderID, x.ProductID, x.StoreID });
                    table.ForeignKey(
                        name: "FK_Orders_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Stores_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Stores",
                        principalColumn: "StoreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdersHistory",
                columns: table => new
                {
                    CustomerID = table.Column<string>(nullable: false),
                    OrderID = table.Column<int>(nullable: false),
                    OrderID1 = table.Column<int>(nullable: true),
                    OrderProductID = table.Column<int>(nullable: true),
                    OrderStoreID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersHistory", x => new { x.CustomerID, x.OrderID });
                    table.UniqueConstraint("AK_OrdersHistory_OrderID", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_OrdersHistory_Orders_OrderID1_OrderProductID_OrderStoreID",
                        columns: x => new { x.OrderID1, x.OrderProductID, x.OrderStoreID },
                        principalTable: "Orders",
                        principalColumns: new[] { "OrderID", "ProductID", "StoreID" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductID",
                table: "Orders",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StoreID",
                table: "Orders",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersHistory_OrderID1_OrderProductID_OrderStoreID",
                table: "OrdersHistory",
                columns: new[] { "OrderID1", "OrderProductID", "OrderStoreID" },
                unique: true,
                filter: "[OrderID1] IS NOT NULL AND [OrderProductID] IS NOT NULL AND [OrderStoreID] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdersHistory");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
