using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    iso3 = table.Column<string>(nullable: true),
                    country_name = table.Column<string>(nullable: true),
                    iso2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CovidCountryData",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    country_id = table.Column<int>(nullable: false),
                    report_date = table.Column<DateTime>(nullable: false),
                    db_update_date = table.Column<DateTime>(nullable: false),
                    confirmed_cases = table.Column<decimal>(type: "decimal(20, 5)", nullable: true),
                    death_cases = table.Column<decimal>(type: "decimal(20,5)", nullable: true),
                    recovered = table.Column<decimal>(type: "decimal(20,5)", nullable: true),
                    active_cases = table.Column<decimal>(type: "decimal(20,5)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CovidCountryData", x => x.id);
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "CovidCountryData");
        }
    }
}
