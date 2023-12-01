using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace HotelWizard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix);
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            //builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));



            var app = builder.Build();
           // app.UseRequestLocalization();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "autorisationRoute",
					pattern: "AutorisationF/Autorisation",
					defaults: new { controller = "HomeController", action = "Autorisation" });

				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
			app.Run();




        }
    }
}