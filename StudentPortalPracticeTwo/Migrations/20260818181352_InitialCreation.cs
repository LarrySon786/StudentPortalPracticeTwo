using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudentPortalPracticeTwo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
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
                    ApprovedStatus = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationDb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Credits = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DegreeDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DegreeDb", x => x.Id);
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
                name: "TermDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Season = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    AvailableToRegisterClasses = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermDb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AcademicHistoryModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    HighschoolTranscriptFileName = table.Column<string>(type: "text", nullable: false),
                    HighschoolTranscript = table.Column<byte[]>(type: "bytea", nullable: false),
                    CollegeTranscriptFileName = table.Column<string>(type: "text", nullable: false),
                    CollegeTranscript = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicHistoryModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicHistoryModel_ApplicationDb_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "ApplicationDb",
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

            migrationBuilder.CreateTable(
                name: "EssayDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    ResponseOne = table.Column<string>(type: "character varying(700)", maxLength: 700, nullable: false),
                    ResponseTwo = table.Column<string>(type: "character varying(700)", maxLength: 700, nullable: false),
                    ResponseThree = table.Column<string>(type: "character varying(700)", maxLength: 700, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "StudentContactDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    AltPhone = table.Column<string>(type: "text", nullable: true)
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
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Race = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    CitizenshipCountry = table.Column<int>(type: "integer", nullable: false),
                    StreetOneAddress = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    StreetTwoAddress = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StateOrProvince = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Zipcode = table.Column<int>(type: "integer", nullable: false)
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FinalApplicationId = table.Column<int>(type: "integer", nullable: true),
                    IdentityUserId = table.Column<string>(type: "text", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDb_ApplicationDb_FinalApplicationId",
                        column: x => x.FinalApplicationId,
                        principalTable: "ApplicationDb",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserDb_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseDegree",
                columns: table => new
                {
                    CoursesId = table.Column<int>(type: "integer", nullable: false),
                    DegreesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDegree", x => new { x.CoursesId, x.DegreesId });
                    table.ForeignKey(
                        name: "FK_CourseDegree_CourseDb_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "CourseDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseDegree_DegreeDb_DegreesId",
                        column: x => x.DegreesId,
                        principalTable: "DegreeDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DraftAcademicHistoryDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DraftApplicationId = table.Column<int>(type: "integer", nullable: false),
                    HighschoolTranscriptFileName = table.Column<string>(type: "text", nullable: false),
                    HighschoolTranscript = table.Column<byte[]>(type: "bytea", nullable: true),
                    CollegeTranscriptFileName = table.Column<string>(type: "text", nullable: false),
                    CollegeTranscript = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftAcademicHistoryDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftAcademicHistoryDb_DraftApplicationDb_DraftApplicationId",
                        column: x => x.DraftApplicationId,
                        principalTable: "DraftApplicationDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "DraftStudentContact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    AltPhone = table.Column<string>(type: "text", nullable: true)
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
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Race = table.Column<int>(type: "integer", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    CitizenshipCountry = table.Column<int>(type: "integer", nullable: true),
                    StreetOneAddress = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    StreetTwoAddress = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StateOrProvince = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Zipcode = table.Column<int>(type: "integer", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "ClassSessionDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CourseId = table.Column<int>(type: "integer", nullable: false),
                    TermId = table.Column<int>(type: "integer", nullable: false),
                    Instructor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    CurrentCount = table.Column<int>(type: "integer", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSessionDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassSessionDb_CourseDb_CourseId",
                        column: x => x.CourseId,
                        principalTable: "CourseDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSessionDb_TermDb_TermId",
                        column: x => x.TermId,
                        principalTable: "TermDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DraftStudentProgram",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    SelectedProgramId = table.Column<int>(type: "integer", nullable: true),
                    StartTermId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftStudentProgram", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftStudentProgram_DegreeDb_SelectedProgramId",
                        column: x => x.SelectedProgramId,
                        principalTable: "DegreeDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DraftStudentProgram_DraftApplicationDb_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "DraftApplicationDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DraftStudentProgram_TermDb_StartTermId",
                        column: x => x.StartTermId,
                        principalTable: "TermDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentProgramDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    SelectedProgramId = table.Column<int>(type: "integer", nullable: false),
                    StartTermId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentProgramDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentProgramDb_ApplicationDb_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "ApplicationDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentProgramDb_DegreeDb_SelectedProgramId",
                        column: x => x.SelectedProgramId,
                        principalTable: "DegreeDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentProgramDb_TermDb_StartTermId",
                        column: x => x.StartTermId,
                        principalTable: "TermDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserContactDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContactDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserContactDb_UserDb_UserId",
                        column: x => x.UserId,
                        principalTable: "UserDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserEmergencyDb",
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
                    table.PrimaryKey("PK_UserEmergencyDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEmergencyDb_UserDb_UserId",
                        column: x => x.UserId,
                        principalTable: "UserDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProgram",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DegreeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProgram", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProgram_DegreeDb_DegreeId",
                        column: x => x.DegreeId,
                        principalTable: "DegreeDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProgram_UserDb_UserId",
                        column: x => x.UserId,
                        principalTable: "UserDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassSessionUserProgramModel",
                columns: table => new
                {
                    CurrentSessionsId = table.Column<int>(type: "integer", nullable: false),
                    StudentProgramModelsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSessionUserProgramModel", x => new { x.CurrentSessionsId, x.StudentProgramModelsId });
                    table.ForeignKey(
                        name: "FK_ClassSessionUserProgramModel_ClassSessionDb_CurrentSessions~",
                        column: x => x.CurrentSessionsId,
                        principalTable: "ClassSessionDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSessionUserProgramModel_UserProgram_StudentProgramMode~",
                        column: x => x.StudentProgramModelsId,
                        principalTable: "UserProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassSessionUserProgramModel1",
                columns: table => new
                {
                    RegisteredSessionsId = table.Column<int>(type: "integer", nullable: false),
                    RegisteredStudentProgramModelsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSessionUserProgramModel1", x => new { x.RegisteredSessionsId, x.RegisteredStudentProgramModelsId });
                    table.ForeignKey(
                        name: "FK_ClassSessionUserProgramModel1_ClassSessionDb_RegisteredSess~",
                        column: x => x.RegisteredSessionsId,
                        principalTable: "ClassSessionDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSessionUserProgramModel1_UserProgram_RegisteredStudent~",
                        column: x => x.RegisteredStudentProgramModelsId,
                        principalTable: "UserProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseUserProgramModel",
                columns: table => new
                {
                    CompletedCoursesId = table.Column<int>(type: "integer", nullable: false),
                    UserProgramModelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseUserProgramModel", x => new { x.CompletedCoursesId, x.UserProgramModelId });
                    table.ForeignKey(
                        name: "FK_CourseUserProgramModel_CourseDb_CompletedCoursesId",
                        column: x => x.CompletedCoursesId,
                        principalTable: "CourseDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseUserProgramModel_UserProgram_UserProgramModelId",
                        column: x => x.UserProgramModelId,
                        principalTable: "UserProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicHistoryModel_ApplicationId",
                table: "AcademicHistoryModel",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessionDb_CourseId",
                table: "ClassSessionDb",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessionDb_TermId",
                table: "ClassSessionDb",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessionUserProgramModel_StudentProgramModelsId",
                table: "ClassSessionUserProgramModel",
                column: "StudentProgramModelsId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessionUserProgramModel1_RegisteredStudentProgramModel~",
                table: "ClassSessionUserProgramModel1",
                column: "RegisteredStudentProgramModelsId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDegree_DegreesId",
                table: "CourseDegree",
                column: "DegreesId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseUserProgramModel_UserProgramModelId",
                table: "CourseUserProgramModel",
                column: "UserProgramModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftAcademicHistoryDb_DraftApplicationId",
                table: "DraftAcademicHistoryDb",
                column: "DraftApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DraftEmergencyContact_DraftApplicationId",
                table: "DraftEmergencyContact",
                column: "DraftApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftEssayDb_DraftApplicationId",
                table: "DraftEssayDb",
                column: "DraftApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DraftStudentProgram_ApplicationId",
                table: "DraftStudentProgram",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DraftStudentProgram_SelectedProgramId",
                table: "DraftStudentProgram",
                column: "SelectedProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftStudentProgram_StartTermId",
                table: "DraftStudentProgram",
                column: "StartTermId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContactDb_ApplicationId",
                table: "EmergencyContactDb",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_EssayDb_ApplicationId",
                table: "EssayDb",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgramDb_ApplicationId",
                table: "StudentProgramDb",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgramDb_SelectedProgramId",
                table: "StudentProgramDb",
                column: "SelectedProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgramDb_StartTermId",
                table: "StudentProgramDb",
                column: "StartTermId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContactDb_UserId",
                table: "UserContactDb",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDb_FinalApplicationId",
                table: "UserDb",
                column: "FinalApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDb_IdentityUserId",
                table: "UserDb",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEmergencyDb_UserId",
                table: "UserEmergencyDb",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgram_DegreeId",
                table: "UserProgram",
                column: "DegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgram_UserId",
                table: "UserProgram",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicHistoryModel");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ClassSessionUserProgramModel");

            migrationBuilder.DropTable(
                name: "ClassSessionUserProgramModel1");

            migrationBuilder.DropTable(
                name: "CourseDegree");

            migrationBuilder.DropTable(
                name: "CourseUserProgramModel");

            migrationBuilder.DropTable(
                name: "DraftAcademicHistoryDb");

            migrationBuilder.DropTable(
                name: "DraftEmergencyContact");

            migrationBuilder.DropTable(
                name: "DraftEssayDb");

            migrationBuilder.DropTable(
                name: "DraftStudentContact");

            migrationBuilder.DropTable(
                name: "DraftStudentInfoDb");

            migrationBuilder.DropTable(
                name: "DraftStudentProgram");

            migrationBuilder.DropTable(
                name: "EmergencyContactDb");

            migrationBuilder.DropTable(
                name: "EssayDb");

            migrationBuilder.DropTable(
                name: "StudentContactDb");

            migrationBuilder.DropTable(
                name: "StudentInfoDb");

            migrationBuilder.DropTable(
                name: "StudentProgramDb");

            migrationBuilder.DropTable(
                name: "UserContactDb");

            migrationBuilder.DropTable(
                name: "UserEmergencyDb");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ClassSessionDb");

            migrationBuilder.DropTable(
                name: "UserProgram");

            migrationBuilder.DropTable(
                name: "DraftApplicationDb");

            migrationBuilder.DropTable(
                name: "CourseDb");

            migrationBuilder.DropTable(
                name: "TermDb");

            migrationBuilder.DropTable(
                name: "DegreeDb");

            migrationBuilder.DropTable(
                name: "UserDb");

            migrationBuilder.DropTable(
                name: "ApplicationDb");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
