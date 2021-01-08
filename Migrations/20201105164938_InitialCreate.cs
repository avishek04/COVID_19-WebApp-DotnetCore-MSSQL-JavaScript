using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace COVID_19.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(maxLength: 50, nullable: true),
                    CountryCode = table.Column<string>(maxLength: 3, nullable: true),
                    Capital = table.Column<string>(maxLength: 50, nullable: true),
                    Population = table.Column<int>(nullable: false),
                    HumanDevelopmentIndex_HDI = table.Column<int>(nullable: false),
                    Density = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurveyQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(nullable: true),
                    SurveyQuestionType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 20, nullable: true),
                    LastName = table.Column<string>(maxLength: 20, nullable: true),
                    UserName = table.Column<string>(maxLength: 20, nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Age = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurveyUserData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    SurveyDate = table.Column<DateTime>(nullable: false),
                    SurveyQuestionsType = table.Column<int>(nullable: true),
                    SurveyQuestion1 = table.Column<bool>(nullable: false),
                    SurveyQuestion2 = table.Column<bool>(nullable: false),
                    SurveyQuestion3 = table.Column<bool>(nullable: false),
                    SurveyQuestion4 = table.Column<bool>(nullable: false),
                    SurveyQuestion5 = table.Column<bool>(nullable: false),
                    SurveyQuestion6 = table.Column<bool>(nullable: false),
                    SurveyQuestion7 = table.Column<bool>(nullable: false),
                    SurveyQuestion8 = table.Column<bool>(nullable: false),
                    SurveyQuestion9 = table.Column<bool>(nullable: false),
                    SurveyQuestion10 = table.Column<bool>(nullable: false),
                    SurveyQuestion11 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyUserData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyUserData_UserMaster_UserId",
                        column: x => x.UserId,
                        principalTable: "UserMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyUserData_UserId",
                table: "SurveyUserData",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "SurveyQuestions");

            migrationBuilder.DropTable(
                name: "SurveyUserData");

            migrationBuilder.DropTable(
                name: "UserMaster");
        }
    }
}
