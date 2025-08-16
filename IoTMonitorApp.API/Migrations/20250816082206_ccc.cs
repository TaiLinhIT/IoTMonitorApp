using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTMonitorApp.API.Migrations
{
    /// <inheritdoc />
    public partial class ccc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Products",
                newName: "ProductUrl");

            migrationBuilder.AddColumn<int>(
                name: "UrlListId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlListId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductUrl",
                table: "Products",
                newName: "ImageUrl");
        }
    }
}
