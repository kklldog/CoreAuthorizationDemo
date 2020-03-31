using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreAuthorization.Policy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreAuthorization
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/account/Login";
            });
            services.AddSingleton<IAuthorizationHandler, LastNameHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("女老师", policy =>
                   policy.RequireAssertion((context) =>
                      {
                          var isTeacher = context.User.HasClaim((c) =>
                          {
                              return c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == "老师";
                          });
                          var isFemale = context.User.HasClaim((c) =>
                          {
                              return c.Type == "Sex" && c.Value == "女";
                          });

                          return isTeacher && isFemale;
                      }
                    )
                 );
                options.AddPolicy("王老师", policy =>
                   policy.AddRequirements(new LastNamRequirement { LastName = "王" })
                 );
            });
            services.AddControllersWithViews();
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
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
