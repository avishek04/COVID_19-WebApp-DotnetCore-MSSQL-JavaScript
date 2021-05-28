using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace COVID_19.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "covidcountrydata",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_covidcountrydata", x => x.id);
                });

        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.DropTable(
                name: "covidcountrydata");
        }
    }
}
