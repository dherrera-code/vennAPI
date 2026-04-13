using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vennAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserModelId",
                table: "RoomMembers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "UserAvailability",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Hour = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAvailability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAvailability_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomMembers_UserModelId",
                table: "RoomMembers",
                column: "UserModelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAvailability_UserId",
                table: "UserAvailability",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomMembers_Users_UserModelId",
                table: "RoomMembers",
                column: "UserModelId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomMembers_Users_UserModelId",
                table: "RoomMembers");

            migrationBuilder.DropTable(
                name: "UserAvailability");

            migrationBuilder.DropIndex(
                name: "IX_RoomMembers_UserModelId",
                table: "RoomMembers");

            migrationBuilder.AlterColumn<int>(
                name: "UserModelId",
                table: "RoomMembers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
