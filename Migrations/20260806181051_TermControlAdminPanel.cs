using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class TermControlAdminPanel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSessionDb_Term_TermId",
                table: "ClassSessionDb");

            migrationBuilder.DropForeignKey(
                name: "FK_DraftStudentProgram_Term_StartTermId",
                table: "DraftStudentProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgram_Term_StartTermId",
                table: "StudentProgram");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Term",
                table: "Term");

            migrationBuilder.RenameTable(
                name: "Term",
                newName: "TermDb");

            migrationBuilder.AddColumn<bool>(
                name: "AvailableToRegisterClasses",
                table: "TermDb",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TermDb",
                table: "TermDb",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSessionDb_TermDb_TermId",
                table: "ClassSessionDb",
                column: "TermId",
                principalTable: "TermDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DraftStudentProgram_TermDb_StartTermId",
                table: "DraftStudentProgram",
                column: "StartTermId",
                principalTable: "TermDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgram_TermDb_StartTermId",
                table: "StudentProgram",
                column: "StartTermId",
                principalTable: "TermDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSessionDb_TermDb_TermId",
                table: "ClassSessionDb");

            migrationBuilder.DropForeignKey(
                name: "FK_DraftStudentProgram_TermDb_StartTermId",
                table: "DraftStudentProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgram_TermDb_StartTermId",
                table: "StudentProgram");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TermDb",
                table: "TermDb");

            migrationBuilder.DropColumn(
                name: "AvailableToRegisterClasses",
                table: "TermDb");

            migrationBuilder.RenameTable(
                name: "TermDb",
                newName: "Term");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Term",
                table: "Term",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSessionDb_Term_TermId",
                table: "ClassSessionDb",
                column: "TermId",
                principalTable: "Term",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DraftStudentProgram_Term_StartTermId",
                table: "DraftStudentProgram",
                column: "StartTermId",
                principalTable: "Term",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgram_Term_StartTermId",
                table: "StudentProgram",
                column: "StartTermId",
                principalTable: "Term",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
