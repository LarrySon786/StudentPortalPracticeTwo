using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class ProgramSelectApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Term",
                table: "ClassSessionDb");

            migrationBuilder.AddColumn<int>(
                name: "TermId",
                table: "ClassSessionDb",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Term",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Season = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Term", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DraftStudentProgram",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    SelectedProgramId = table.Column<int>(type: "integer", nullable: true),
                    StartTermId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftStudentProgram", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftStudentProgram_DegreeDb_SelectedProgramId",
                        column: x => x.SelectedProgramId,
                        principalTable: "DegreeDb",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DraftStudentProgram_DraftApplicationDb_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "DraftApplicationDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DraftStudentProgram_Term_StartTermId",
                        column: x => x.StartTermId,
                        principalTable: "Term",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentProgram",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    SelectedProgramId = table.Column<int>(type: "integer", nullable: false),
                    StartTermId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentProgram", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentProgram_ApplicationDb_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "ApplicationDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentProgram_DegreeDb_SelectedProgramId",
                        column: x => x.SelectedProgramId,
                        principalTable: "DegreeDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentProgram_Term_StartTermId",
                        column: x => x.StartTermId,
                        principalTable: "Term",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessionDb_TermId",
                table: "ClassSessionDb",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftStudentProgram_ApplicationId",
                table: "DraftStudentProgram",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DraftStudentProgram_SelectedProgramId",
                table: "DraftStudentProgram",
                column: "SelectedProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftStudentProgram_StartTermId",
                table: "DraftStudentProgram",
                column: "StartTermId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgram_ApplicationId",
                table: "StudentProgram",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgram_SelectedProgramId",
                table: "StudentProgram",
                column: "SelectedProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgram_StartTermId",
                table: "StudentProgram",
                column: "StartTermId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSessionDb_Term_TermId",
                table: "ClassSessionDb",
                column: "TermId",
                principalTable: "Term",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSessionDb_Term_TermId",
                table: "ClassSessionDb");

            migrationBuilder.DropTable(
                name: "DraftStudentProgram");

            migrationBuilder.DropTable(
                name: "StudentProgram");

            migrationBuilder.DropTable(
                name: "Term");

            migrationBuilder.DropIndex(
                name: "IX_ClassSessionDb_TermId",
                table: "ClassSessionDb");

            migrationBuilder.DropColumn(
                name: "TermId",
                table: "ClassSessionDb");

            migrationBuilder.AddColumn<string>(
                name: "Term",
                table: "ClassSessionDb",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
