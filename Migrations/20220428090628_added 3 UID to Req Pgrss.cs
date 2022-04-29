using Microsoft.EntityFrameworkCore.Migrations;

namespace RPFBE.Migrations
{
    public partial class added3UIDtoReqPgrss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UIDFour",
                table: "RequisitionProgress",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UIDThree",
                table: "RequisitionProgress",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UIDTwo",
                table: "RequisitionProgress",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UIDFour",
                table: "RequisitionProgress");

            migrationBuilder.DropColumn(
                name: "UIDThree",
                table: "RequisitionProgress");

            migrationBuilder.DropColumn(
                name: "UIDTwo",
                table: "RequisitionProgress");
        }
    }
}
