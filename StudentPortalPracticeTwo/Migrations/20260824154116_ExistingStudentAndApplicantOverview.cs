using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class ExistingStudentAndApplicantOverview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicHistoryModel_ApplicationDb_ApplicationId",
                table: "AcademicHistoryModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AcademicHistoryModel",
                table: "AcademicHistoryModel");

            migrationBuilder.RenameTable(
                name: "AcademicHistoryModel",
                newName: "AcademicHistoryDb");

            migrationBuilder.RenameIndex(
                name: "IX_AcademicHistoryModel_ApplicationId",
                table: "AcademicHistoryDb",
                newName: "IX_AcademicHistoryDb_ApplicationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AcademicHistoryDb",
                table: "AcademicHistoryDb",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicHistoryDb_ApplicationDb_ApplicationId",
                table: "AcademicHistoryDb",
                column: "ApplicationId",
                principalTable: "ApplicationDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicHistoryDb_ApplicationDb_ApplicationId",
                table: "AcademicHistoryDb");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AcademicHistoryDb",
                table: "AcademicHistoryDb");

            migrationBuilder.RenameTable(
                name: "AcademicHistoryDb",
                newName: "AcademicHistoryModel");

            migrationBuilder.RenameIndex(
                name: "IX_AcademicHistoryDb_ApplicationId",
                table: "AcademicHistoryModel",
                newName: "IX_AcademicHistoryModel_ApplicationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AcademicHistoryModel",
                table: "AcademicHistoryModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicHistoryModel_ApplicationDb_ApplicationId",
                table: "AcademicHistoryModel",
                column: "ApplicationId",
                principalTable: "ApplicationDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
