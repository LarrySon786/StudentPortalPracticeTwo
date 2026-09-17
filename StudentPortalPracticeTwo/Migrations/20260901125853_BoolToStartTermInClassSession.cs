using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class BoolToStartTermInClassSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ClassStarted",
                table: "ClassSessionDb",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassStarted",
                table: "ClassSessionDb");
        }
    }
}
