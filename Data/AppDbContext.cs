using Microsoft.EntityFrameworkCore;
using COVID_19.Models;

namespace COVID_19.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<CovidData> covidcountrydata { get; set; }
        public DbSet<CovidVaccineData> covidVaccineData { get; set; }
        public DbSet<Country> country { get; set; }
        public DbSet<Questions> questions { get; set; }
        public DbSet<SurveyResponse> surveyResponse { get; set; }
        public DbSet<SiteTracking> siteTracking { get; set; }
        //public DbSet<User> users { get; set; }

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
