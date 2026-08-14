using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class FixedMyProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProgram_DegreeDb_DegreeId",
                table: "UserProgram");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgram_DegreeDb_DegreeId",
                table: "UserProgram",
                column: "DegreeId",
                principalTable: "DegreeDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProgram_DegreeDb_DegreeId",
                table: "UserProgram");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgram_DegreeDb_DegreeId",
                table: "UserProgram",
                column: "DegreeId",
                principalTable: "DegreeDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
