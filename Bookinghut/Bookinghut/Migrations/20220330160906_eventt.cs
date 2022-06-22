using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookinghut.Migrations
{
    public partial class eventt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentEvent");

            migrationBuilder.DropColumn(
                name: "CurentEventID",
                table: "Event");

            migrationBuilder.AddColumn<int>(
                name: "RoleID",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "Event",
                type: "bit",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleID", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Organizer" },
                    { 3, "Customer" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserID", "Adress", "FirstName", "LastName", "Mail", "PasswordHash", "PasswordSalt", "Phone", "RoleID", "Username" },
                values: new object[] { 1, "adressadmin", "Admin", "Admin", "admin.admin@gmail.com", null, null, "000111222", 1, null });

            migrationBuilder.InsertData(
                table: "Venue",
                columns: new[] { "VenueID", "Name" },
                values: new object[] { 1, "Venue1" });

            migrationBuilder.InsertData(
                table: "Event",
                columns: new[] { "EventID", "Name", "NumberOfTickets", "Price", "Timing", "VenueID", "status" },
                values: new object[] { 1, "Name>Event", 2, 12f, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "EventID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "RoleID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "RoleID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "RoleID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "UserID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Venue",
                keyColumn: "VenueID",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Event");

            migrationBuilder.AddColumn<int>(
                name: "CurentEventID",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CurrentEvent",
                columns: table => new
                {
                    CurentEventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentEvent", x => x.CurentEventID);
                    table.ForeignKey(
                        name: "FK_CurrentEvent_Event_EventID",
                        column: x => x.EventID,
                        principalTable: "Event",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrentEvent_EventID",
                table: "CurrentEvent",
                column: "EventID",
                unique: true);
        }
    }
}
