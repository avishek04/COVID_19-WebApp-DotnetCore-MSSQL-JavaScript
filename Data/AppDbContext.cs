using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using COVID_19.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace COVID_19.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<CovidData> covidcountrydata { get; set; }
        public DbSet<Country> country { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<SurveyQuestions>()
            //    .Property(c => c.SurveyQuestionType)
            //    .HasConversion<int>();

            //modelBuilder.Entity<SurveyUserData>()
            //    .Property(c => c.SurveyQuestionsType)
            //    .HasConversion<int>();

            //modelBuilder.Entity<SurveyUserData>()
            //    .HasOne<UserMaster>()
            //    .WithMany()
            //    .HasForeignKey(s => s.UserId);

            base.OnModelCreating(modelBuilder);

        }
    }
}
