using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookinghut.Migrations
{
    public partial class status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "Event",
                newName: "Status");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Event",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Event",
                newName: "status");

            migrationBuilder.AlterColumn<bool>(
                name: "status",
                table: "Event",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
