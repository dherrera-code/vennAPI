using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vennAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountCreationTimeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserAvailability_UserId",
                table: "UserAvailability");

            migrationBuilder.AddColumn<DateTime>(
                name: "AccountCreated",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_UserAvailability_UserId_Day_Hour",
                table: "UserAvailability",
                columns: new[] { "UserId", "Day", "Hour" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friends_ReceiverId",
                table: "Friends",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_RequesterId",
                table: "Friends",
                column: "RequesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Users_ReceiverId",
                table: "Friends",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Users_RequesterId",
                table: "Friends",
                column: "RequesterId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_ReceiverId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_RequesterId",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_UserAvailability_UserId_Day_Hour",
                table: "UserAvailability");

            migrationBuilder.DropIndex(
                name: "IX_Friends_ReceiverId",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_Friends_RequesterId",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "AccountCreated",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_UserAvailability_UserId",
                table: "UserAvailability",
                column: "UserId");
        }
    }
}
