using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wishler.Migrations
{
    /// <inheritdoc />
    public partial class AddOtherNameColumnToGroupParticipantTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherName",
                table: "GroupParticipants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherName",
                table: "GroupParticipants");
        }
    }
}
