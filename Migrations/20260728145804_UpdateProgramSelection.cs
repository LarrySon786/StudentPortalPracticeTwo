using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProgramSelection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DraftStudentProgram_DegreeDb_SelectedProgramId",
                table: "DraftStudentProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_DraftStudentProgram_Term_StartTermId",
                table: "DraftStudentProgram");

            migrationBuilder.AlterColumn<int>(
                name: "StartTermId",
                table: "DraftStudentProgram",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SelectedProgramId",
                table: "DraftStudentProgram",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DraftStudentProgram_DegreeDb_SelectedProgramId",
                table: "DraftStudentProgram",
                column: "SelectedProgramId",
                principalTable: "DegreeDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DraftStudentProgram_Term_StartTermId",
                table: "DraftStudentProgram",
                column: "StartTermId",
                principalTable: "Term",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DraftStudentProgram_DegreeDb_SelectedProgramId",
                table: "DraftStudentProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_DraftStudentProgram_Term_StartTermId",
                table: "DraftStudentProgram");

            migrationBuilder.AlterColumn<int>(
                name: "StartTermId",
                table: "DraftStudentProgram",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "SelectedProgramId",
                table: "DraftStudentProgram",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_DraftStudentProgram_DegreeDb_SelectedProgramId",
                table: "DraftStudentProgram",
                column: "SelectedProgramId",
                principalTable: "DegreeDb",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DraftStudentProgram_Term_StartTermId",
                table: "DraftStudentProgram",
                column: "StartTermId",
                principalTable: "Term",
                principalColumn: "Id");
        }
    }
}
