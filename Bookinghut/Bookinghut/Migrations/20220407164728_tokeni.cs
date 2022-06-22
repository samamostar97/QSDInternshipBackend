using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookinghut.Migrations
{
    public partial class tokeni : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role_RoleID",
                table: "UserRole");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_RoleID",
                table: "UserRole");

            migrationBuilder.RenameColumn(
                name: "RoleID",
                table: "User",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "User",
                newName: "VerificationToken");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "UserRole",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "AcceptTerms",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordReset",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResetToken",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpires",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Verified",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserID",
                keyValue: 1,
                column: "Role",
                value: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserID",
                table: "RefreshToken",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "AcceptTerms",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PasswordReset",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ResetToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpires",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Verified",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "VerificationToken",
                table: "User",
                newName: "PasswordSalt");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "User",
                newName: "RoleID");

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleID);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleID", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Organizer" },
                    { 3, "Customer" }
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserID",
                keyValue: 1,
                column: "RoleID",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleID",
                table: "UserRole",
                column: "RoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_RoleID",
                table: "UserRole",
                column: "RoleID",
                principalTable: "Role",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
