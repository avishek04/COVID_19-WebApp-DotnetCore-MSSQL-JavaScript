using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using COVID_19.Data;
using Microsoft.EntityFrameworkCore;
using COVID_19.Data.Repository;
using COVID_19.CoreApiClient;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace COVID_19
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("AppDbContext")));

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //        .AddJwtBearer(options =>
            //        {
            //            options.TokenValidationParameters = new TokenValidationParameters
            //            {
            //                ValidateIssuerSigningKey = true,
            //                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
            //                    .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
            //                ValidateIssuer = false,
            //                ValidateAudience = false
            //            };
            //        });
            services.AddControllers();
            services.AddMemoryCache();
            services.AddHttpClient();
            //services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICovidDataRepository, CovidDataRepository>();
            services.AddScoped<ICovidVaccineRepository, CovidVaccineRepository>();
            services.AddScoped<IInformationRepository, InformationRepository>();
            services.AddScoped<ICovidDataClient, CovidDataClient>();
            services.AddScoped<INewsAPIClient, NewsAPIClient>();
            services.AddScoped<ISurveyRepository, SurveyRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
