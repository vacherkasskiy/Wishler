using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wishler.Migrations
{
    /// <inheritdoc />
    public partial class AddIsStartedColumnToGroupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStarted",
                table: "Groups",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStarted",
                table: "Groups");
        }
    }
}
