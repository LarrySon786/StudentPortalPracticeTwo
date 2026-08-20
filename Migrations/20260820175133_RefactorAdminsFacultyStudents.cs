using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class RefactorAdminsFacultyStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instructor",
                table: "ClassSessionDb");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "UserDb",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "UserDb",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "test",
                table: "UserDb",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "ClassSessionDb",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessionDb_InstructorId",
                table: "ClassSessionDb",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSessionDb_UserDb_InstructorId",
                table: "ClassSessionDb",
                column: "InstructorId",
                principalTable: "UserDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSessionDb_UserDb_InstructorId",
                table: "ClassSessionDb");

            migrationBuilder.DropIndex(
                name: "IX_ClassSessionDb_InstructorId",
                table: "ClassSessionDb");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "UserDb");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "UserDb");

            migrationBuilder.DropColumn(
                name: "test",
                table: "UserDb");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "ClassSessionDb");

            migrationBuilder.AddColumn<string>(
                name: "Instructor",
                table: "ClassSessionDb",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
