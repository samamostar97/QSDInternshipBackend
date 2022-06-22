using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookinghut.Migrations
{
    public partial class hash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserID",
                keyValue: 1,
                column: "Verified",
                value: new DateTime(2022, 4, 17, 11, 49, 57, 712, DateTimeKind.Local).AddTicks(2064));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserID",
                keyValue: 1,
                column: "Verified",
                value: new DateTime(2022, 4, 16, 16, 52, 3, 738, DateTimeKind.Local).AddTicks(4154));
        }
    }
}
