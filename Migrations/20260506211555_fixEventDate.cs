using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vennAPI.Migrations
{
    /// <inheritdoc />
    public partial class fixEventDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsRoomActive",
                table: "Rooms",
                type: "bit",
                nullable: false,
                computedColumnSql: "CASE WHEN EventDate >= GETUTCDATE() THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComputedColumnSql: "CASE WHEN EventDate >= GETUTCDATE() THEN 1 ELSE 0 END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsRoomActive",
                table: "Rooms",
                type: "bit",
                nullable: false,
                computedColumnSql: "CASE WHEN EventDate >= GETUTCDATE() THEN 1 ELSE 0 END",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComputedColumnSql: "CASE WHEN EventDate >= GETUTCDATE() THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END");
        }
    }
}
