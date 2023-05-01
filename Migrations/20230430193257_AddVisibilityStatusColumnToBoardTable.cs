using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wishler.Migrations
{
    /// <inheritdoc />
    public partial class AddVisibilityStatusColumnToBoardTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VisibilityStatus",
                table: "Boards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisibilityStatus",
                table: "Boards");
        }
    }
}
