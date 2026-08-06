using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class AcademicHistoryApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgram_ApplicationDb_ApplicationId",
                table: "StudentProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgram_DegreeDb_SelectedProgramId",
                table: "StudentProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgram_TermDb_StartTermId",
                table: "StudentProgram");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentProgram",
                table: "StudentProgram");

            migrationBuilder.RenameTable(
                name: "StudentProgram",
                newName: "StudentProgramDb");

            migrationBuilder.RenameIndex(
                name: "IX_StudentProgram_StartTermId",
                table: "StudentProgramDb",
                newName: "IX_StudentProgramDb_StartTermId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentProgram_SelectedProgramId",
                table: "StudentProgramDb",
                newName: "IX_StudentProgramDb_SelectedProgramId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentProgram_ApplicationId",
                table: "StudentProgramDb",
                newName: "IX_StudentProgramDb_ApplicationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentProgramDb",
                table: "StudentProgramDb",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AcademicHistoryModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    HighschoolTranscriptFileName = table.Column<string>(type: "text", nullable: false),
                    HighschoolTranscript = table.Column<byte[]>(type: "bytea", nullable: false),
                    CollegeTranscriptFileName = table.Column<string>(type: "text", nullable: false),
                    CollegeTranscript = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicHistoryModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicHistoryModel_ApplicationDb_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "ApplicationDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DraftAcademicHistoryDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DraftApplicationId = table.Column<int>(type: "integer", nullable: false),
                    HighschoolTranscriptFileName = table.Column<string>(type: "text", nullable: false),
                    HighschoolTranscript = table.Column<byte[]>(type: "bytea", nullable: true),
                    CollegeTranscriptFileName = table.Column<string>(type: "text", nullable: false),
                    CollegeTranscript = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftAcademicHistoryDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftAcademicHistoryDb_DraftApplicationDb_DraftApplicationId",
                        column: x => x.DraftApplicationId,
                        principalTable: "DraftApplicationDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicHistoryModel_ApplicationId",
                table: "AcademicHistoryModel",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DraftAcademicHistoryDb_DraftApplicationId",
                table: "DraftAcademicHistoryDb",
                column: "DraftApplicationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgramDb_ApplicationDb_ApplicationId",
                table: "StudentProgramDb",
                column: "ApplicationId",
                principalTable: "ApplicationDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgramDb_DegreeDb_SelectedProgramId",
                table: "StudentProgramDb",
                column: "SelectedProgramId",
                principalTable: "DegreeDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgramDb_TermDb_StartTermId",
                table: "StudentProgramDb",
                column: "StartTermId",
                principalTable: "TermDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgramDb_ApplicationDb_ApplicationId",
                table: "StudentProgramDb");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgramDb_DegreeDb_SelectedProgramId",
                table: "StudentProgramDb");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgramDb_TermDb_StartTermId",
                table: "StudentProgramDb");

            migrationBuilder.DropTable(
                name: "AcademicHistoryModel");

            migrationBuilder.DropTable(
                name: "DraftAcademicHistoryDb");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentProgramDb",
                table: "StudentProgramDb");

            migrationBuilder.RenameTable(
                name: "StudentProgramDb",
                newName: "StudentProgram");

            migrationBuilder.RenameIndex(
                name: "IX_StudentProgramDb_StartTermId",
                table: "StudentProgram",
                newName: "IX_StudentProgram_StartTermId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentProgramDb_SelectedProgramId",
                table: "StudentProgram",
                newName: "IX_StudentProgram_SelectedProgramId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentProgramDb_ApplicationId",
                table: "StudentProgram",
                newName: "IX_StudentProgram_ApplicationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentProgram",
                table: "StudentProgram",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgram_ApplicationDb_ApplicationId",
                table: "StudentProgram",
                column: "ApplicationId",
                principalTable: "ApplicationDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgram_DegreeDb_SelectedProgramId",
                table: "StudentProgram",
                column: "SelectedProgramId",
                principalTable: "DegreeDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgram_TermDb_StartTermId",
                table: "StudentProgram",
                column: "StartTermId",
                principalTable: "TermDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
