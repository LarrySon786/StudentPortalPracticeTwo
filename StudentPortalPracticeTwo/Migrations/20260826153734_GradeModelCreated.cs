using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class GradeModelCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentsDb_UserProgram_StudentProgramId",
                table: "AssignmentsDb");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentsDb_StudentProgramId",
                table: "AssignmentsDb");

            migrationBuilder.DropColumn(
                name: "ClassSessionId",
                table: "UserProgram");

            migrationBuilder.DropColumn(
                name: "ScoredPoints",
                table: "AssignmentsDb");

            migrationBuilder.DropColumn(
                name: "StudentProgramId",
                table: "AssignmentsDb");

            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                table: "AssignmentsDb",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "GradeDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentProgramId = table.Column<int>(type: "integer", nullable: false),
                    AssignmentId = table.Column<int>(type: "integer", nullable: false),
                    SessionId = table.Column<int>(type: "integer", nullable: false),
                    ScoredPoints = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeDb_AssignmentsDb_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "AssignmentsDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeDb_ClassSessionDb_SessionId",
                        column: x => x.SessionId,
                        principalTable: "ClassSessionDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeDb_UserProgram_StudentProgramId",
                        column: x => x.StudentProgramId,
                        principalTable: "UserProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GradeDb_AssignmentId",
                table: "GradeDb",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeDb_SessionId",
                table: "GradeDb",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeDb_StudentProgramId",
                table: "GradeDb",
                column: "StudentProgramId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GradeDb");

            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "AssignmentsDb");

            migrationBuilder.AddColumn<int>(
                name: "ClassSessionId",
                table: "UserProgram",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScoredPoints",
                table: "AssignmentsDb",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentProgramId",
                table: "AssignmentsDb",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsDb_StudentProgramId",
                table: "AssignmentsDb",
                column: "StudentProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentsDb_UserProgram_StudentProgramId",
                table: "AssignmentsDb",
                column: "StudentProgramId",
                principalTable: "UserProgram",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
