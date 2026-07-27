using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class EmergencyContactApplicationUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AltPhone",
                table: "StudentContactDb",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AltPhone",
                table: "DraftStudentContact",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DraftEmergencyContact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DraftApplicationId = table.Column<int>(type: "integer", nullable: false),
                    ContactName = table.Column<string>(type: "text", nullable: false),
                    Relationship = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftEmergencyContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftEmergencyContact_DraftApplicationDb_DraftApplicationId",
                        column: x => x.DraftApplicationId,
                        principalTable: "DraftApplicationDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyContactDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    ContactName = table.Column<string>(type: "text", nullable: false),
                    Relationship = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyContactDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmergencyContactDb_ApplicationDb_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "ApplicationDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DraftEmergencyContact_DraftApplicationId",
                table: "DraftEmergencyContact",
                column: "DraftApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContactDb_ApplicationId",
                table: "EmergencyContactDb",
                column: "ApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DraftEmergencyContact");

            migrationBuilder.DropTable(
                name: "EmergencyContactDb");

            migrationBuilder.DropColumn(
                name: "AltPhone",
                table: "StudentContactDb");

            migrationBuilder.DropColumn(
                name: "AltPhone",
                table: "DraftStudentContact");
        }
    }
}
