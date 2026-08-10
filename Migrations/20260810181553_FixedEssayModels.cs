using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class FixedEssayModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResponseTwo",
                table: "EssayDb",
                type: "character varying(700)",
                maxLength: 700,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ResponseThree",
                table: "EssayDb",
                type: "character varying(700)",
                maxLength: 700,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1500)",
                oldMaxLength: 1500);

            migrationBuilder.AlterColumn<string>(
                name: "ResponseOne",
                table: "EssayDb",
                type: "character varying(700)",
                maxLength: 700,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1500)",
                oldMaxLength: 1500);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResponseTwo",
                table: "EssayDb",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(700)",
                oldMaxLength: 700);

            migrationBuilder.AlterColumn<string>(
                name: "ResponseThree",
                table: "EssayDb",
                type: "character varying(1500)",
                maxLength: 1500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(700)",
                oldMaxLength: 700);

            migrationBuilder.AlterColumn<string>(
                name: "ResponseOne",
                table: "EssayDb",
                type: "character varying(1500)",
                maxLength: 1500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(700)",
                oldMaxLength: 700);
        }
    }
}
