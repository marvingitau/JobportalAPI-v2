using Microsoft.EntityFrameworkCore.Migrations;

namespace RPFBE.Migrations
{
    public partial class AddedRequiprogressv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RequisitionProgress",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ClosingDate",
                table: "RequisitionProgress",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobGrade",
                table: "RequisitionProgress",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobNo",
                table: "RequisitionProgress",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "RequisitionProgress",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProgressStatus",
                table: "RequisitionProgress",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RequestedEmployees",
                table: "RequisitionProgress",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingDate",
                table: "RequisitionProgress");

            migrationBuilder.DropColumn(
                name: "JobGrade",
                table: "RequisitionProgress");

            migrationBuilder.DropColumn(
                name: "JobNo",
                table: "RequisitionProgress");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "RequisitionProgress");

            migrationBuilder.DropColumn(
                name: "ProgressStatus",
                table: "RequisitionProgress");

            migrationBuilder.DropColumn(
                name: "RequestedEmployees",
                table: "RequisitionProgress");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "RequisitionProgress",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
