using Microsoft.EntityFrameworkCore.Migrations;

namespace RPFBE.Migrations
{
    public partial class addedsomefieldtoprofile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentSalary",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpectedSalary",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HighestEducation",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WillingtoRelocate",
                table: "Profiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentSalary",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ExpectedSalary",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "HighestEducation",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "WillingtoRelocate",
                table: "Profiles");
        }
    }
}
