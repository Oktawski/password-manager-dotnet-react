using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Migrations
{
    public partial class UpdateEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passwords_AspNetUsers_ApplicationUserId",
                table: "Passwords");

            migrationBuilder.DropForeignKey(
                name: "FK_Passwords_User_UserId",
                table: "Passwords");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Passwords_ApplicationUserId",
                table: "Passwords");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Passwords");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Passwords",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Passwords_AspNetUsers_UserId",
                table: "Passwords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passwords_AspNetUsers_UserId",
                table: "Passwords");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Passwords",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Passwords",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Passwords_ApplicationUserId",
                table: "Passwords",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Passwords_AspNetUsers_ApplicationUserId",
                table: "Passwords",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Passwords_User_UserId",
                table: "Passwords",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
