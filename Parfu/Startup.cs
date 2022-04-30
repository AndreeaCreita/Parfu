using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Parfu.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfu
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
            services.AddMemoryCache();
            services.AddSession(options =>
            {
                //options.IdleTimeout = TimeSpan.FromSeconds(2)
                //options.IdleTimeout = TimeSpan.FromDays(2)
            });
            services.AddControllersWithViews();
            //A dependency is an object that can be used (for exemple as a service)
            //It is any object that another object requires


            //register services and dependencies in the configureService method
            services.AddDbContext<ParfuContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ParfuContext")));
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

            app.UseRouting();
            app.UseSession();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //goes from most specific to least specific
                //frontend Pages-> pagina de pornire
                endpoints.MapControllerRoute(
                    "pages", 
                    "{slug?}",
                    defaults: new {controller = "Pages", action = "Page"}
                   );

                endpoints.MapControllerRoute(
                    "products", 
                    "products/{categorySlug}",
                    defaults: new { controller = "Products", action = "ProductsByCategory" }
                   );


                //ia din Areas / Admin 
                endpoints.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
             );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
