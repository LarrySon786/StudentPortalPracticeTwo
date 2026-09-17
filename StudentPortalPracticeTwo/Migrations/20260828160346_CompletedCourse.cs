using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class CompletedCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseUserProgramModel");

            migrationBuilder.AlterColumn<decimal>(
                name: "ScoredPoints",
                table: "GradeDb",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "CompletedCourseDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CourseId = table.Column<int>(type: "integer", nullable: false),
                    SessionTakenId = table.Column<int>(type: "integer", nullable: false),
                    StudentProgramId = table.Column<int>(type: "integer", nullable: false),
                    Grade = table.Column<decimal>(type: "numeric", nullable: false),
                    GPA = table.Column<decimal>(type: "numeric", nullable: false),
                    DateCompleted = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedCourseDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedCourseDb_ClassSessionDb_SessionTakenId",
                        column: x => x.SessionTakenId,
                        principalTable: "ClassSessionDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompletedCourseDb_CourseDb_CourseId",
                        column: x => x.CourseId,
                        principalTable: "CourseDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompletedCourseDb_UserProgram_StudentProgramId",
                        column: x => x.StudentProgramId,
                        principalTable: "UserProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedCourseDb_CourseId",
                table: "CompletedCourseDb",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedCourseDb_SessionTakenId",
                table: "CompletedCourseDb",
                column: "SessionTakenId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedCourseDb_StudentProgramId",
                table: "CompletedCourseDb",
                column: "StudentProgramId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedCourseDb");

            migrationBuilder.AlterColumn<int>(
                name: "ScoredPoints",
                table: "GradeDb",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.CreateTable(
                name: "CourseUserProgramModel",
                columns: table => new
                {
                    CompletedCoursesId = table.Column<int>(type: "integer", nullable: false),
                    UserProgramModelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseUserProgramModel", x => new { x.CompletedCoursesId, x.UserProgramModelId });
                    table.ForeignKey(
                        name: "FK_CourseUserProgramModel_CourseDb_CompletedCoursesId",
                        column: x => x.CompletedCoursesId,
                        principalTable: "CourseDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseUserProgramModel_UserProgram_UserProgramModelId",
                        column: x => x.UserProgramModelId,
                        principalTable: "UserProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseUserProgramModel_UserProgramModelId",
                table: "CourseUserProgramModel",
                column: "UserProgramModelId");
        }
    }
}
