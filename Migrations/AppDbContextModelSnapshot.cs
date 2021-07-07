﻿// <auto-generated />
using System;
using COVID_19.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace COVID_19.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("COVID_19.Models.Country", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("continent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("country_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("iso2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("iso3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("population")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("country");
                });

            modelBuilder.Entity("COVID_19.Models.CovidData", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal?>("biweekly_cases")
                        .HasColumnType("decimal(20,5)");

                    b.Property<decimal?>("biweekly_deaths")
                        .HasColumnType("decimal(20,5)");

                    b.Property<int>("country_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("db_update_date")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("new_cases")
                        .HasColumnType("decimal(20, 5)");

                    b.Property<decimal?>("new_deaths")
                        .HasColumnType("decimal(20,5)");

                    b.Property<DateTime>("report_date")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("total_cases")
                        .HasColumnType("decimal(20,5)");

                    b.Property<decimal?>("total_deaths")
                        .HasColumnType("decimal(20,5)");

                    b.Property<decimal?>("weekly_cases")
                        .HasColumnType("decimal(20,5)");

                    b.Property<decimal?>("weekly_deaths")
                        .HasColumnType("decimal(20,5)");

                    b.HasKey("id");

                    b.ToTable("covidcountrydata");
                });

            modelBuilder.Entity("COVID_19.Models.CovidVaccineData", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("country_id")
                        .HasColumnType("int");

                    b.Property<decimal?>("daily_vaccinations")
                        .HasColumnType("decimal(20, 5)");

                    b.Property<decimal?>("daily_vaccinations_per_million")
                        .HasColumnType("decimal(20,5)");

                    b.Property<DateTime>("db_update_date_vaccine")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("people_fully_vaccinated")
                        .HasColumnType("decimal(20,5)");

                    b.Property<decimal?>("people_fully_vaccinated_per_hundred")
                        .HasColumnType("decimal(20,5)");

                    b.Property<decimal?>("people_vaccinated")
                        .HasColumnType("decimal(20,5)");

                    b.Property<decimal?>("people_vaccinated_per_hundred")
                        .HasColumnType("decimal(20,5)");

                    b.Property<decimal?>("total_vaccinations")
                        .HasColumnType("decimal(20,5)");

                    b.Property<decimal?>("total_vaccinations_per_hundred")
                        .HasColumnType("decimal(20,5)");

                    b.Property<DateTime>("vaccine_report_date")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.ToTable("covidVaccineData");
                });

            modelBuilder.Entity("COVID_19.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("users");
                });
#pragma warning restore 612, 618
        }
    }
}
