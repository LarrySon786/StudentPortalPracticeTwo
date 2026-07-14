using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationDb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DraftApplicationDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftApplicationDb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentContactDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentContactDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentContactDb_ApplicationDb_Id",
                        column: x => x.Id,
                        principalTable: "ApplicationDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentInfoDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentInfoDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentInfoDb_ApplicationDb_Id",
                        column: x => x.Id,
                        principalTable: "ApplicationDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DraftStudentContact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftStudentContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftStudentContact_DraftApplicationDb_Id",
                        column: x => x.Id,
                        principalTable: "DraftApplicationDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DraftStudentInfoDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftStudentInfoDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftStudentInfoDb_DraftApplicationDb_Id",
                        column: x => x.Id,
                        principalTable: "DraftApplicationDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DraftStudentContact");

            migrationBuilder.DropTable(
                name: "DraftStudentInfoDb");

            migrationBuilder.DropTable(
                name: "StudentContactDb");

            migrationBuilder.DropTable(
                name: "StudentInfoDb");

            migrationBuilder.DropTable(
                name: "DraftApplicationDb");

            migrationBuilder.DropTable(
                name: "ApplicationDb");
        }
    }
}
