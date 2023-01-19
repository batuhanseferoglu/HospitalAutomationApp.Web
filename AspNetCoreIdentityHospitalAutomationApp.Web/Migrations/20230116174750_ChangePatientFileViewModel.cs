using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreIdentityHospitalAutomationApp.Web.Migrations
{
    /// <inheritdoc />
    public partial class ChangePatientFileViewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_AspNetUsers_userId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_userId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Files");

            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Files",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_userId",
                table: "Files",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_AspNetUsers_userId",
                table: "Files",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
