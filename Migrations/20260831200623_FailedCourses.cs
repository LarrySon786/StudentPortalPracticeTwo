using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class FailedCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassSessionUserProgramModel2");

            migrationBuilder.AddColumn<int>(
                name: "ClassSessionId",
                table: "CompletedCourseDb",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FailedCourse",
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
                    table.PrimaryKey("PK_FailedCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FailedCourse_ClassSessionDb_SessionTakenId",
                        column: x => x.SessionTakenId,
                        principalTable: "ClassSessionDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FailedCourse_CourseDb_CourseId",
                        column: x => x.CourseId,
                        principalTable: "CourseDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FailedCourse_UserProgram_StudentProgramId",
                        column: x => x.StudentProgramId,
                        principalTable: "UserProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedCourseDb_ClassSessionId",
                table: "CompletedCourseDb",
                column: "ClassSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_FailedCourse_CourseId",
                table: "FailedCourse",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_FailedCourse_SessionTakenId",
                table: "FailedCourse",
                column: "SessionTakenId");

            migrationBuilder.CreateIndex(
                name: "IX_FailedCourse_StudentProgramId",
                table: "FailedCourse",
                column: "StudentProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedCourseDb_ClassSessionDb_ClassSessionId",
                table: "CompletedCourseDb",
                column: "ClassSessionId",
                principalTable: "ClassSessionDb",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletedCourseDb_ClassSessionDb_ClassSessionId",
                table: "CompletedCourseDb");

            migrationBuilder.DropTable(
                name: "FailedCourse");

            migrationBuilder.DropIndex(
                name: "IX_CompletedCourseDb_ClassSessionId",
                table: "CompletedCourseDb");

            migrationBuilder.DropColumn(
                name: "ClassSessionId",
                table: "CompletedCourseDb");

            migrationBuilder.CreateTable(
                name: "ClassSessionUserProgramModel2",
                columns: table => new
                {
                    FailedSessionsId = table.Column<int>(type: "integer", nullable: false),
                    UserProgramModelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSessionUserProgramModel2", x => new { x.FailedSessionsId, x.UserProgramModelId });
                    table.ForeignKey(
                        name: "FK_ClassSessionUserProgramModel2_ClassSessionDb_FailedSessions~",
                        column: x => x.FailedSessionsId,
                        principalTable: "ClassSessionDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSessionUserProgramModel2_UserProgram_UserProgramModelId",
                        column: x => x.UserProgramModelId,
                        principalTable: "UserProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessionUserProgramModel2_UserProgramModelId",
                table: "ClassSessionUserProgramModel2",
                column: "UserProgramModelId");
        }
    }
}
