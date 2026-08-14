using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class SampleDataAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DegreeId",
                table: "CourseDb");

            migrationBuilder.CreateTable(
                name: "UserEmergencyContactModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ContactName = table.Column<string>(type: "text", nullable: false),
                    Relationship = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEmergencyContactModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEmergencyContactModel_UserDb_UserId",
                        column: x => x.UserId,
                        principalTable: "UserDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProgramModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DegreeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProgramModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProgramModel_DegreeDb_DegreeId",
                        column: x => x.DegreeId,
                        principalTable: "DegreeDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProgramModel_UserDb_UserId",
                        column: x => x.UserId,
                        principalTable: "UserDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEmergencyContactModel_UserId",
                table: "UserEmergencyContactModel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgramModel_DegreeId",
                table: "UserProgramModel",
                column: "DegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgramModel_UserId",
                table: "UserProgramModel",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEmergencyContactModel");

            migrationBuilder.DropTable(
                name: "UserProgramModel");

            migrationBuilder.AddColumn<List<int>>(
                name: "DegreeId",
                table: "CourseDb",
                type: "integer[]",
                nullable: false);
        }
    }
}
