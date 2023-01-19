using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreIdentityHospitalAutomationApp.Web.Migrations
{
    /// <inheritdoc />
    public partial class test4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Start",
                table: "Events",
                newName: "StartEvent");

            migrationBuilder.RenameColumn(
                name: "End",
                table: "Events",
                newName: "EndEvent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartEvent",
                table: "Events",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "EndEvent",
                table: "Events",
                newName: "End");
        }
    }
}
