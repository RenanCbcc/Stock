using Microsoft.EntityFrameworkCore.Migrations;

namespace Estoque.Migrations
{
    public partial class ItemDiscout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Discound",
                table: "Items",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Value",
                table: "Items",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discound",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Items");
        }
    }
}
