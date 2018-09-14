using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItinerariesApi.Models;
using ItinerariesApi.Models.DAL;
using ItinerariesApi.Models.Gateway;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ItinerariesApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ItinerariesApi:ConnectionString"]);
            });
            services.AddTransient<ICategoryRepository, EFCategoryRepository>();
            services.AddTransient<ICountryRepository, EFCountryRepository>();
            services.AddTransient<ICityRepository, EFCityRepository>();
            services.AddTransient<ITouristAttractionRepository, EFTouristAttractionRepository>();
            services.AddTransient<ITouristAttractionSuggestionRepository, EFTouristAttractionSuggestionRepository>();
            services.AddTransient<IAdministratorRepository, EFAdministratorRepository>();
            services.AddSingleton<IMailSender, SendGridMailer>();
            services.AddTransient<IItineraryGenerator, OptimizedItineraryGenerator>();
            services.AddResponseCaching();
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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseResponseCaching();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
