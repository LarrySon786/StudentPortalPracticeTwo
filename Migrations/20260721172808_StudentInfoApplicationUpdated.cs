using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class StudentInfoApplicationUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "StudentInfoDb",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "StudentInfoDb",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "CitizenshipCountry",
                table: "StudentInfoDb",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "StudentInfoDb",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "StudentInfoDb",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "StudentInfoDb",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Race",
                table: "StudentInfoDb",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StateOrProvince",
                table: "StudentInfoDb",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StreetOneAddress",
                table: "StudentInfoDb",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StreetTwoAddress",
                table: "StudentInfoDb",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Zipcode",
                table: "StudentInfoDb",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "DraftStudentInfoDb",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "DraftStudentInfoDb",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CitizenshipCountry",
                table: "DraftStudentInfoDb",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "DraftStudentInfoDb",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "DraftStudentInfoDb",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "DraftStudentInfoDb",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Race",
                table: "DraftStudentInfoDb",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateOrProvince",
                table: "DraftStudentInfoDb",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetOneAddress",
                table: "DraftStudentInfoDb",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetTwoAddress",
                table: "DraftStudentInfoDb",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Zipcode",
                table: "DraftStudentInfoDb",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CitizenshipCountry",
                table: "StudentInfoDb");

            migrationBuilder.DropColumn(
                name: "City",
                table: "StudentInfoDb");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "StudentInfoDb");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "StudentInfoDb");

            migrationBuilder.DropColumn(
                name: "Race",
                table: "StudentInfoDb");

            migrationBuilder.DropColumn(
                name: "StateOrProvince",
                table: "StudentInfoDb");

            migrationBuilder.DropColumn(
                name: "StreetOneAddress",
                table: "StudentInfoDb");

            migrationBuilder.DropColumn(
                name: "StreetTwoAddress",
                table: "StudentInfoDb");

            migrationBuilder.DropColumn(
                name: "Zipcode",
                table: "StudentInfoDb");

            migrationBuilder.DropColumn(
                name: "CitizenshipCountry",
                table: "DraftStudentInfoDb");

            migrationBuilder.DropColumn(
                name: "City",
                table: "DraftStudentInfoDb");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "DraftStudentInfoDb");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "DraftStudentInfoDb");

            migrationBuilder.DropColumn(
                name: "Race",
                table: "DraftStudentInfoDb");

            migrationBuilder.DropColumn(
                name: "StateOrProvince",
                table: "DraftStudentInfoDb");

            migrationBuilder.DropColumn(
                name: "StreetOneAddress",
                table: "DraftStudentInfoDb");

            migrationBuilder.DropColumn(
                name: "StreetTwoAddress",
                table: "DraftStudentInfoDb");

            migrationBuilder.DropColumn(
                name: "Zipcode",
                table: "DraftStudentInfoDb");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "StudentInfoDb",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "StudentInfoDb",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "DraftStudentInfoDb",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "DraftStudentInfoDb",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
