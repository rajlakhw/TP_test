using Data;
//using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Services;
using flowPlusExternal.Models;
using System;
using Data.Repositories;
using Services.Middlewares;
using Extensions;
using LinguisticData;

namespace flowPlusExternal
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
            services.Configure<SmartSettings>(Configuration.GetSection(SmartSettings.SectionName));


            // Note: This line is for demonstration purposes only, I would not recommend using this as a shorthand approach for accessing settings
            // While having to type '.Value' everywhere is driving me nuts (>_<), using this method means reloaded appSettings.json from disk will not work
            services.AddSingleton(s => s.GetRequiredService<IOptions<SmartSettings>>().Value);

            //services.AddTransient(s => s.GetRequiredService<IOptions<SmartSettings>>().Value);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<TPCoreProductionContext>(
            //   options => options.UseSqlServer(Configuration.GetConnectionString("DEV")));
            //services.AddDbContext<TPCoreProductionContext>(
            //   options => options.UseSqlServer(Configuration.GetConnectionString("PRODUCTION")));
            services.AddDbContext<TPCoreProductionContext>(
               options => options.UseSqlServer(Configuration.GetConnectionString("PRODUCTION")));
            services.AddDbContext<TPLinguisticProductionContext>(
               options => options.UseSqlServer(Configuration.GetConnectionString("LinguisticDatabasesPRODUCTION")));

            //services.AddDbContext<Data.QA.TPCoreQAandCustomerWSTestingNEWContext>(
            //   options => options.UseSqlServer(Configuration.GetConnectionString("QAandCustomerWSTesting")));
            //services.AddIdentity<ExtranetUsersTemp>(options => options.SignIn.RequireConfirmedAccount = false)               
            //    .AddEntityFrameworkStores<TPCoreProductionContext>();

            //services.AddIdentity<IdentityUser, IdentityRole>(
            //        options => {
            //            options.SignIn.RequireConfirmedAccount = false;

            //            //Other options go here
            //        }
            //        )
            //        .AddEntityFrameworkStores<TPCoreProductionContext>();
            // Register Sevrices
            services.AddConventionalServices();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(Data.QA.Repositories.IRepository<>), typeof(Data.QA.Repositories.EfRepository<>));
            services.AddScoped(typeof(Data.Repositories.IRepository<>), typeof(Data.Repositories.EfRepository<>));
            services.AddScoped(typeof(LinguisticData.Repositories.ILinguisticRepository<>), typeof(LinguisticData.Repositories.EfLinguisticRepository<>));

            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<NullClaimExceptionFilter>();
            });

            services
              .AddMvc()
              .AddRazorPagesOptions(options =>
              {
                  options.Conventions.AddAreaPageRoute(
                      areaName: "Identity",
                      pageName: "/Account/Login",
                      route: "");
              });
            //    services.AddIdentity<Data.ExtranetUsersTemp, IdentityRole>(
            //options =>
            //{
            //    options.SignIn.RequireConfirmedAccount = false;

            //    //Other options go here
            //}
            //).AddEntityFrameworkStores<Data.TPCoreProductionContext>();

            services.AddIdentity<Data.ExtranetUsersTemp, IdentityRole>(
       options =>
       {
           options.SignIn.RequireConfirmedAccount = false;

           //Other options go here
           options.Password.RequireDigit = false;
           options.Password.RequireLowercase = true;
           options.Password.RequireUppercase = false;
           options.Password.RequiredLength = 8;
           options.Password.RequireNonAlphanumeric = false;
       }
       ).AddEntityFrameworkStores<Data.TPCoreProductionContext>()
        .AddTokenProvider<DataProtectorTokenProvider<Data.ExtranetUsersTemp>>(TokenOptions.DefaultProvider);


            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                //options.Cookie.Expiration 

                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                //options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}");
                endpoints.MapRazorPages();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
            });
        }
    }
}
