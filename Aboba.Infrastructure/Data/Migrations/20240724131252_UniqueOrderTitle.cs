using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aboba.Infrastucture.Data.Migrations
{
    /// <inheritdoc />
    public partial class UniqueOrderTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_Title",
                table: "Orders",
                column: "Title",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_Title",
                table: "Orders");
        }
    }
}
