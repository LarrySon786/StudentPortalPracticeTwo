using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserModelDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEmergencyContactModel_UserDb_UserId",
                table: "UserEmergencyContactModel");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgramModel_DegreeDb_DegreeId",
                table: "UserProgramModel");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgramModel_UserDb_UserId",
                table: "UserProgramModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProgramModel",
                table: "UserProgramModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserEmergencyContactModel",
                table: "UserEmergencyContactModel");

            migrationBuilder.RenameTable(
                name: "UserProgramModel",
                newName: "UserProgram");

            migrationBuilder.RenameTable(
                name: "UserEmergencyContactModel",
                newName: "UserEmergencyDb");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgramModel_UserId",
                table: "UserProgram",
                newName: "IX_UserProgram_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgramModel_DegreeId",
                table: "UserProgram",
                newName: "IX_UserProgram_DegreeId");

            migrationBuilder.RenameIndex(
                name: "IX_UserEmergencyContactModel_UserId",
                table: "UserEmergencyDb",
                newName: "IX_UserEmergencyDb_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProgram",
                table: "UserProgram",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserEmergencyDb",
                table: "UserEmergencyDb",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEmergencyDb_UserDb_UserId",
                table: "UserEmergencyDb",
                column: "UserId",
                principalTable: "UserDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgram_DegreeDb_DegreeId",
                table: "UserProgram",
                column: "DegreeId",
                principalTable: "DegreeDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgram_UserDb_UserId",
                table: "UserProgram",
                column: "UserId",
                principalTable: "UserDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEmergencyDb_UserDb_UserId",
                table: "UserEmergencyDb");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgram_DegreeDb_DegreeId",
                table: "UserProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgram_UserDb_UserId",
                table: "UserProgram");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProgram",
                table: "UserProgram");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserEmergencyDb",
                table: "UserEmergencyDb");

            migrationBuilder.RenameTable(
                name: "UserProgram",
                newName: "UserProgramModel");

            migrationBuilder.RenameTable(
                name: "UserEmergencyDb",
                newName: "UserEmergencyContactModel");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgram_UserId",
                table: "UserProgramModel",
                newName: "IX_UserProgramModel_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgram_DegreeId",
                table: "UserProgramModel",
                newName: "IX_UserProgramModel_DegreeId");

            migrationBuilder.RenameIndex(
                name: "IX_UserEmergencyDb_UserId",
                table: "UserEmergencyContactModel",
                newName: "IX_UserEmergencyContactModel_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProgramModel",
                table: "UserProgramModel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserEmergencyContactModel",
                table: "UserEmergencyContactModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEmergencyContactModel_UserDb_UserId",
                table: "UserEmergencyContactModel",
                column: "UserId",
                principalTable: "UserDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgramModel_DegreeDb_DegreeId",
                table: "UserProgramModel",
                column: "DegreeId",
                principalTable: "DegreeDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgramModel_UserDb_UserId",
                table: "UserProgramModel",
                column: "UserId",
                principalTable: "UserDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
