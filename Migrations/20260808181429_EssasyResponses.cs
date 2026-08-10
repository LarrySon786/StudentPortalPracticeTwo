using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class EssasyResponses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DraftEssayDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DraftApplicationId = table.Column<int>(type: "integer", nullable: false),
                    ResponseOne = table.Column<string>(type: "text", nullable: false),
                    ResponseTwo = table.Column<string>(type: "text", nullable: false),
                    ResponseThree = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftEssayDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftEssayDb_DraftApplicationDb_DraftApplicationId",
                        column: x => x.DraftApplicationId,
                        principalTable: "DraftApplicationDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EssayDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    ResponseOne = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    ResponseTwo = table.Column<string>(type: "text", nullable: false),
                    ResponseThree = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EssayDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EssayDb_ApplicationDb_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "ApplicationDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DraftEssayDb_DraftApplicationId",
                table: "DraftEssayDb",
                column: "DraftApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EssayDb_ApplicationId",
                table: "EssayDb",
                column: "ApplicationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DraftEssayDb");

            migrationBuilder.DropTable(
                name: "EssayDb");
        }
    }
}
