using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserFinalApplicationNullability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDb_ApplicationDb_FinalApplicationId",
                table: "UserDb");

            migrationBuilder.AlterColumn<int>(
                name: "FinalApplicationId",
                table: "UserDb",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDb_ApplicationDb_FinalApplicationId",
                table: "UserDb",
                column: "FinalApplicationId",
                principalTable: "ApplicationDb",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDb_ApplicationDb_FinalApplicationId",
                table: "UserDb");

            migrationBuilder.AlterColumn<int>(
                name: "FinalApplicationId",
                table: "UserDb",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDb_ApplicationDb_FinalApplicationId",
                table: "UserDb",
                column: "FinalApplicationId",
                principalTable: "ApplicationDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
