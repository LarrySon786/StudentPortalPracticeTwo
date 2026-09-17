using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class SupportTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupportTicketsDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Topic = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTicketsDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportTicketsDb_UserDb_StudentId",
                        column: x => x.StudentId,
                        principalTable: "UserDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResponseTicket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SupportTicketId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    StudentTicketInput = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseTicket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResponseTicket_SupportTicketsDb_SupportTicketId",
                        column: x => x.SupportTicketId,
                        principalTable: "SupportTicketsDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResponseTicket_UserDb_UserId",
                        column: x => x.UserId,
                        principalTable: "UserDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResponseTicket_SupportTicketId",
                table: "ResponseTicket",
                column: "SupportTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponseTicket_UserId",
                table: "ResponseTicket",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicketsDb_StudentId",
                table: "SupportTicketsDb",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResponseTicket");

            migrationBuilder.DropTable(
                name: "SupportTicketsDb");
        }
    }
}
