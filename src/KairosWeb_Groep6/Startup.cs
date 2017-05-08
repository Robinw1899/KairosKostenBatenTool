using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using KairosWeb_Groep6.Data;
using KairosWeb_Groep6.Data.Repositories;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;

namespace KairosWeb_Groep6
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets("aspnet-KairosWeb_Groep6-b1133a05-0e80-4e06-909f-a7d404128be2");
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                           options.UseMySql(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<KairosIdentityErrorDescriber>();

            services.AddMvc(
                options =>
                {
                    options.ModelBindingMessageProvider
                        .ValueMustBeANumberAccessor = s => $"{s} mag enkel een getal bevatten. Let op: decimalen schrijf je met" +
                                                           " een punt i.p.v. een komma!";
                });
            services.AddSession();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddScoped<AnalyseFilter>();
            services.AddScoped<JobcoachFilter>();
            services.AddScoped<IJobcoachRepository, JobcoachRepository>();
            services.AddScoped<IDepartementRepository, DepartementRepository>();
            services.AddScoped<IAnalyseRepository, AnalyseRepository>();
            services.AddScoped<IWerkgeverRepository, WerkgeverRepository>();
            services.AddScoped<IContactPersoonRepository, ContactPersoonRepository>();
            services.AddScoped<IIntroductietekstRepository, IntroductietekstRepository>();
            services.AddScoped<IDoelgroepRepository, DoelgroepRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager, IJobcoachRepository gebruikerRepository,
            IDepartementRepository departementRepository, IAnalyseRepository analyseRepository, IWerkgeverRepository werkgeverRepository,
            IIntroductietekstRepository introductietekstRepository, IDoelgroepRepository doelgroepRepository)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
                app.UseExceptionHandler("/Kairos/Error");
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Kairos/Error");
                app.UseStatusCodePages();
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseIdentity();
          
            app.UseSession();

            app.UseRequestLocalization(BuildLocalizationOptions());

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Kairos}/{action=Index}/{id?}");
            });

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            DataInitializer initializer = new DataInitializer(context, userManager, gebruikerRepository,
                                                              departementRepository, analyseRepository, werkgeverRepository,
                                                             introductietekstRepository, doelgroepRepository);
            //initializer.InitializeIntrotekst();
            //initializer.InitializeDoelgroepen();
            initializer.InitializeData().Wait();
        }

        private RequestLocalizationOptions BuildLocalizationOptions()
        {
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("nl-BE"),
                new CultureInfo("fr-FR"),
                new CultureInfo("en-US")
            };

            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("nl-BE"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            return options;
        }
    }
}
