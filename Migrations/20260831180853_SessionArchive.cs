using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class SessionArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ArchivedAndClosed",
                table: "ClassSessionDb",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassSessionUserProgramModel2");

            migrationBuilder.DropColumn(
                name: "ArchivedAndClosed",
                table: "ClassSessionDb");
        }
    }
}
