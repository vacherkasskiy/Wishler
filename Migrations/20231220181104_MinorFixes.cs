using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Wishler.Migrations
{
    public partial class MinorFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardOwners_Groups_GroupId",
                table: "BoardOwners");

            migrationBuilder.DropForeignKey(
                name: "FK_BoardOwners_Users_OwnerId",
                table: "BoardOwners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BoardOwners",
                table: "BoardOwners");

            migrationBuilder.RenameTable(
                name: "BoardOwners",
                newName: "GroupOwners");

            migrationBuilder.RenameIndex(
                name: "IX_BoardOwners_OwnerId",
                table: "GroupOwners",
                newName: "IX_GroupOwners_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_BoardOwners_GroupId",
                table: "GroupOwners",
                newName: "IX_GroupOwners_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupOwners",
                table: "GroupOwners",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Wish",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wish", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wish_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wish_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupParticipants_GroupId",
                table: "GroupParticipants",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupParticipants_UserId",
                table: "GroupParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wish_GroupId",
                table: "Wish",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Wish_UserId",
                table: "Wish",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupOwners_Groups_GroupId",
                table: "GroupOwners",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupOwners_Users_OwnerId",
                table: "GroupOwners",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupParticipants_Groups_GroupId",
                table: "GroupParticipants",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupParticipants_Users_UserId",
                table: "GroupParticipants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupOwners_Groups_GroupId",
                table: "GroupOwners");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupOwners_Users_OwnerId",
                table: "GroupOwners");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupParticipants_Groups_GroupId",
                table: "GroupParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupParticipants_Users_UserId",
                table: "GroupParticipants");

            migrationBuilder.DropTable(
                name: "Wish");

            migrationBuilder.DropIndex(
                name: "IX_GroupParticipants_GroupId",
                table: "GroupParticipants");

            migrationBuilder.DropIndex(
                name: "IX_GroupParticipants_UserId",
                table: "GroupParticipants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupOwners",
                table: "GroupOwners");

            migrationBuilder.RenameTable(
                name: "GroupOwners",
                newName: "BoardOwners");

            migrationBuilder.RenameIndex(
                name: "IX_GroupOwners_OwnerId",
                table: "BoardOwners",
                newName: "IX_BoardOwners_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupOwners_GroupId",
                table: "BoardOwners",
                newName: "IX_BoardOwners_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoardOwners",
                table: "BoardOwners",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardOwners_Groups_GroupId",
                table: "BoardOwners",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BoardOwners_Users_OwnerId",
                table: "BoardOwners",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
