using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationEmailVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccessToken",
                table: "DraftApplicationDb",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpires",
                table: "DraftApplicationDb",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerificationCodeExpires",
                table: "DraftApplicationDb",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificationCodeHash",
                table: "DraftApplicationDb",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DraftVerificationCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DraftApplicationId = table.Column<int>(type: "integer", nullable: false),
                    HashedCode = table.Column<string>(type: "text", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Used = table.Column<bool>(type: "boolean", nullable: false),
                    FailedAttempts = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftVerificationCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftVerificationCode_DraftApplicationDb_DraftApplicationId",
                        column: x => x.DraftApplicationId,
                        principalTable: "DraftApplicationDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DraftVerificationCode_DraftApplicationId",
                table: "DraftVerificationCode",
                column: "DraftApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DraftVerificationCode");

            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "DraftApplicationDb");

            migrationBuilder.DropColumn(
                name: "TokenExpires",
                table: "DraftApplicationDb");

            migrationBuilder.DropColumn(
                name: "VerificationCodeExpires",
                table: "DraftApplicationDb");

            migrationBuilder.DropColumn(
                name: "VerificationCodeHash",
                table: "DraftApplicationDb");
        }
    }
}
