using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wishler.Migrations
{
    /// <inheritdoc />
    public partial class AddOtherWishColumnToGroupParticipantTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherWish",
                table: "GroupParticipants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherWish",
                table: "GroupParticipants");
        }
    }
}
