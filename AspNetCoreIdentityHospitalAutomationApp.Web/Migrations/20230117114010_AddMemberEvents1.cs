using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreIdentityHospitalAutomationApp.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberEvents1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "MemberEvents");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "MemberEvents");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "MemberEvents");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "MemberEvents");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "MemberEvents");

            migrationBuilder.RenameColumn(
                name: "BirthDay",
                table: "MemberEvents",
                newName: "SelectedDate");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "MemberEvents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SelectedDate",
                table: "MemberEvents",
                newName: "BirthDay");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "MemberEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "MemberEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "MemberEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "MemberEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "MemberEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "MemberEvents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
