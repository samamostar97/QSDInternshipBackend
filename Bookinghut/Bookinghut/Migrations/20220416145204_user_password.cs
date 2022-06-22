using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookinghut.Migrations
{
    public partial class user_password : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserID",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Username", "Verified" },
                values: new object[] { "gfd6eR2Zm04iyOYuhHUiOsxXsW0=", "admin", new DateTime(2022, 4, 16, 16, 52, 3, 738, DateTimeKind.Local).AddTicks(4154) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserID",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Username", "Verified" },
                values: new object[] { null, null, new DateTime(2022, 4, 14, 21, 28, 55, 624, DateTimeKind.Local).AddTicks(5976) });
        }
    }
}
