using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ML.OnnxRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtahCollisions.Models;

namespace UtahCollisions
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
            services.AddControllersWithViews();

            services.AddDbContext<UtahCollisionsContext>(options =>
            { 
                options.UseMySql(Configuration["CollisionsDBConnection"]);
            });

            // for repository pattern 
            services.AddScoped<iUtahCollisionRepository, EFUtahCollisionsRespository>();

            ///////////////////////////////
            /////ONNX SetUp Service////////
            ///////////////////////////////

            services.AddSingleton<InferenceSession>(
                new InferenceSession("CollisionModel.onnx") //Insert location of ONNX file in the quotes! Ex: "Model/california_housing.onnx")
            );

            ///////////////////////////////
            ///////////////////////////////

            // for authentication users + roles 
            services.AddDbContext<AppIdentityDBContext>(options =>
            {
                options.UseMySql(Configuration["IdentityDBConnection"]);
            });

            // for authentication 
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDBContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Yeet.Cookie";
                config.LoginPath = "/Home/LoginTest";
            });

            

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Admin",
            //        policy => policy.RequireClaim("Admin"));
            //}); 
            // {
            //    var defaultAuthBuilder = new AuthorizationPolicyBuilder();
            //    var defaultAuthPolicy = defaultAuthBuilder
            //        .RequireAuthenticatedUser()
            //        .RequireClaim()
            //       .Build();

            //   config.DefaultPolicy = defaultAuthPolicy;

            //});


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

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy(
                new CookiePolicyOptions
                {
                    Secure = CookieSecurePolicy.Always
                });

            ////////CSP Header/////////////
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy-Report-Only", "default-src 'self'");
                await next();
            });
            ///if you?re trying to apply CSP in an existing website it is quite overwhelming because most of the code 
            ///- inline styles, scripts, events and all may not work. To avoid this issue, you can use Content-Security-Policy-Report-Only 
            ///header with the report URL parameter - it will not ignore the script or resource instead it will display the error to the report URL.
            ///////////////////////////////

            app.UseRouting();


            app.UseAuthentication(); // for authentication
            app.UseAuthorization(); // for authoriation

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });



            IdentitySeedData.EnsurePopulated(app);
        }
    }
}
