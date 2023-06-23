using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DShopAPI.Migrations
{
    public partial class removeProdu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Color",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Color_ProductId",
                table: "Color",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Color_Products_ProductId",
                table: "Color",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Color_Products_ProductId",
                table: "Color");

            migrationBuilder.DropIndex(
                name: "IX_Color_ProductId",
                table: "Color");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Color");
        }
    }
}
