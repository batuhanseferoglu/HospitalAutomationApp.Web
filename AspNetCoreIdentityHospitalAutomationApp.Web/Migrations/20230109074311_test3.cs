using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreIdentityHospitalAutomationApp.Web.Migrations
{
    /// <inheritdoc />
    public partial class test3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndEvent",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "StartEvent",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "End",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Start",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "End",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "Events");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndEvent",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartEvent",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
