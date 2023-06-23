using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DShopAPI.Migrations
{
    public partial class removeProd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "ProductColors");

            migrationBuilder.AddColumn<int>(
                name: "SizeByLetter",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SizeByNumber",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "ProductColors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_ColorId",
                table: "ProductColors",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColors_Color_ColorId",
                table: "ProductColors",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductColors_Color_ColorId",
                table: "ProductColors");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropIndex(
                name: "IX_ProductColors_ColorId",
                table: "ProductColors");

            migrationBuilder.DropColumn(
                name: "SizeByLetter",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SizeByNumber",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "ProductColors");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "ProductColors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
