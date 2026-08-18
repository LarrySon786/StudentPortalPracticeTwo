using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class MyProgramRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassSessionUserProgramModel",
                columns: table => new
                {
                    CurrentSessionsId = table.Column<int>(type: "integer", nullable: false),
                    StudentProgramModelsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSessionUserProgramModel", x => new { x.CurrentSessionsId, x.StudentProgramModelsId });
                    table.ForeignKey(
                        name: "FK_ClassSessionUserProgramModel_ClassSessionDb_CurrentSessions~",
                        column: x => x.CurrentSessionsId,
                        principalTable: "ClassSessionDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSessionUserProgramModel_UserProgram_StudentProgramMode~",
                        column: x => x.StudentProgramModelsId,
                        principalTable: "UserProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassSessionUserProgramModel1",
                columns: table => new
                {
                    RegisteredSessionsId = table.Column<int>(type: "integer", nullable: false),
                    RegisteredStudentProgramModelsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSessionUserProgramModel1", x => new { x.RegisteredSessionsId, x.RegisteredStudentProgramModelsId });
                    table.ForeignKey(
                        name: "FK_ClassSessionUserProgramModel1_ClassSessionDb_RegisteredSess~",
                        column: x => x.RegisteredSessionsId,
                        principalTable: "ClassSessionDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSessionUserProgramModel1_UserProgram_RegisteredStudent~",
                        column: x => x.RegisteredStudentProgramModelsId,
                        principalTable: "UserProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_ClassSessionUserProgramModel_StudentProgramModelsId",
                table: "ClassSessionUserProgramModel",
                column: "StudentProgramModelsId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessionUserProgramModel1_RegisteredStudentProgramModel~",
                table: "ClassSessionUserProgramModel1",
                column: "RegisteredStudentProgramModelsId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseUserProgramModel_UserProgramModelId",
                table: "CourseUserProgramModel",
                column: "UserProgramModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassSessionUserProgramModel");

            migrationBuilder.DropTable(
                name: "ClassSessionUserProgramModel1");

            migrationBuilder.DropTable(
                name: "CourseUserProgramModel");
        }
    }
}
