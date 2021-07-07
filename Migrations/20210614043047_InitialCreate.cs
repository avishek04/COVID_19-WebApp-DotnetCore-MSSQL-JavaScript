using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace COVID_19.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "country",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        country_name = table.Column<string>(nullable: true),
            //        continent = table.Column<string>(nullable: true),
            //        population = table.Column<int>(nullable: false),
            //        iso2 = table.Column<string>(nullable: true),
            //        iso3 = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_country", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "covidcountrydata",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        country_id = table.Column<int>(nullable: false),
            //        report_date = table.Column<DateTime>(nullable: false),
            //        db_update_date = table.Column<DateTime>(nullable: false),
            //        new_cases = table.Column<decimal>(type: "decimal(20, 5)", nullable: true),
            //        new_deaths = table.Column<decimal>(type: "decimal(20,5)", nullable: true),
            //        weekly_cases = table.Column<decimal>(type: "decimal(20,5)", nullable: true),
            //        weekly_deaths = table.Column<decimal>(type: "decimal(20,5)", nullable: true),
            //        biweekly_cases = table.Column<decimal>(type: "decimal(20,5)", nullable: true),
            //        biweekly_deaths = table.Column<decimal>(type: "decimal(20,5)", nullable: true),
            //        total_cases = table.Column<decimal>(type: "decimal(20,5)", nullable: true),
            //        total_deaths = table.Column<decimal>(type: "decimal(20,5)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_covidcountrydata", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "covidVaccineData",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        country_id = table.Column<int>(nullable: false),
            //        vaccine_report_date = table.Column<DateTime>(nullable: false),
            //        db_update_date_vaccine = table.Column<DateTime>(nullable: false),
            //        daily_vaccinations = table.Column<decimal>(type: "decimal(20, 5)", nullable: true),
            //        daily_vaccinations_per_million = table.Column<decimal>(type: "decimal(20,5)", nullable: true),
            //        people_vaccinated = table.Column<decimal>(type: "decimal(20,5)", nullable: true),
            //        people_vaccinated_per_hundred = table.Column<decimal>(type: "decimal(20,5)", nullable: true),
            //        people_fully_vaccinated = table.Column<decimal>(type: "decimal(20,5)", nullable: true),
            //        people_fully_vaccinated_per_hundred = table.Column<decimal>(type: "decimal(20,5)", nullable: true),
            //        total_vaccinations = table.Column<decimal>(type: "decimal(20,5)", nullable: true),
            //        total_vaccinations_per_hundred = table.Column<decimal>(type: "decimal(20,5)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_covidVaccineData", x => x.id);
            //    });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "country");

            migrationBuilder.DropTable(
                name: "covidcountrydata");

            migrationBuilder.DropTable(
                name: "covidVaccineData");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
