using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using HotelWizard.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;

namespace HotelWizard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Локализация
            builder.Services.AddControllersWithViews().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix);
              builder.Services.AddControllersWithViews()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResource));
                }).AddViewLocalization();


            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
          
            builder.Services.AddHttpContextAccessor(); // 
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection)); // подключение к бд

			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		        .AddCookie(options => //CookieAuthenticationOptions
		        {
			        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Autorisation");
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                });
			var app = builder.Build();
           // app.UseRequestLocalization();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("ru"),
                new CultureInfo("uz"),
                new CultureInfo("de")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });



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