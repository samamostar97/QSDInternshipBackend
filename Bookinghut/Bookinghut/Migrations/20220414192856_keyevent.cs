using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookinghut.Migrations
{
    public partial class keyevent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserID",
                keyValue: 1,
                column: "Verified",
                value: new DateTime(2022, 4, 14, 21, 28, 55, 624, DateTimeKind.Local).AddTicks(5976));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserID",
                keyValue: 1,
                column: "Verified",
                value: null);
        }
    }
}
