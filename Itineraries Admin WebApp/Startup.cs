using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ItinerariesAdminWebApp.Models;
using ItinerariesAdminWebApp.Models.DAL;
using ItinerariesAdminWebApp.Models.Gateway;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Itineraries_Admin_WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            //  var builder = new ConfigurationBuilder()
            //.SetBasePath(env.ContentRootPath)
            //.AddJsonFile("appsettings.json",
            //             optional: false,
            //             reloadOnChange: true)
            //.AddEnvironmentVariables();

            //  if (env.IsDevelopment())
            //  {
            //      builder.AddUserSecrets<Startup>();
            //  }

            //  Configuration = builder.Build();
            this.Configuration = configuration;

        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseLazyLoadingProxies()
                    .UseSqlServer(Configuration["ItinerariesAdminWebApp:ConnectionString"]);
            });
            services.AddMvc()
                .AddJsonOptions(
                    options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                        //options.SerializerSettings.DateFormatString = "dd/MM/yyyy";
                    }
                     );
            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
                options.AutomaticAuthentication = false;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddSingleton<ICryptoManager, BCryptEncrypter>();
            services.AddSingleton<IMailSender, SendGridMailer>();
            services.AddTransient<IAdministratorRepository, EFAdministratorRepository>();
            services.AddTransient<ICategoryRepository, EFCategoryRepository>();
            services.AddTransient<ICityRepository, EFCityRepository>();
            services.AddTransient<ICountryRepository, EFCountryRepository>();
            services.AddTransient<IInvitationRepository, EFInvitationRepository>();
            services.AddTransient<ITouristAttractionRepository, EFTouristAttractionRepository>();
            services.AddTransient<ITouristAttractionSuggestionRepository, EFTouristAttractionSuggestionRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseAuthentication();
            //Setup of culture of the app
            var defaultDateCulture = "es-NI";
            var ci = new CultureInfo(defaultDateCulture);
            ci.NumberFormat.NumberDecimalSeparator = ".";
            ci.NumberFormat.NumberGroupSeparator = ",";
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            // Configure the Localization middleware
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(ci),
                SupportedCultures = new List<CultureInfo>
                {
                    ci,
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    ci,
                }
            });
            app.UseMvcWithDefaultRoute();
        }
    }
}
