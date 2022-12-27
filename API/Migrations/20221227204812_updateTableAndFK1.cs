using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class updateTableAndFK1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceHistories_Employees_EmployeeNIK",
                table: "AttendanceHistories");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceHistories_EmployeeNIK",
                table: "AttendanceHistories");

            migrationBuilder.DropColumn(
                name: "EmployeeNIK",
                table: "AttendanceHistories");

            migrationBuilder.AlterColumn<string>(
                name: "NIK",
                table: "AttendanceHistories",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceHistories_NIK",
                table: "AttendanceHistories",
                column: "NIK");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceHistories_Employees_NIK",
                table: "AttendanceHistories",
                column: "NIK",
                principalTable: "Employees",
                principalColumn: "NIK",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceHistories_Employees_NIK",
                table: "AttendanceHistories");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceHistories_NIK",
                table: "AttendanceHistories");

            migrationBuilder.AlterColumn<string>(
                name: "NIK",
                table: "AttendanceHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeNIK",
                table: "AttendanceHistories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceHistories_EmployeeNIK",
                table: "AttendanceHistories",
                column: "EmployeeNIK");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceHistories_Employees_EmployeeNIK",
                table: "AttendanceHistories",
                column: "EmployeeNIK",
                principalTable: "Employees",
                principalColumn: "NIK",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
