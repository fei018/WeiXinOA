using Microsoft.EntityFrameworkCore.Migrations;

namespace WeiXinOA.Migrations
{
    public partial class NotMapped : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Tbl_Volunteer");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Tbl_Volunteer");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Tbl_ElderFamily");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Tbl_Elder");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Tbl_Elder");

            migrationBuilder.AlterColumn<string>(
                name: "ElderIdNumber",
                table: "Tbl_ElderFamily",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Age",
                table: "Tbl_Volunteer",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BirthDate",
                table: "Tbl_Volunteer",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ElderIdNumber",
                table: "Tbl_ElderFamily",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Age",
                table: "Tbl_ElderFamily",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Age",
                table: "Tbl_Elder",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BirthDate",
                table: "Tbl_Elder",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");
        }
    }
}
