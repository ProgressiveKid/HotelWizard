using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using HotelWizard.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.OpenApi.Models;

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

            // Добавление Swagger в приложение
            builder.Services.AddEndpointsApiExplorer();  // Добавляем поддержку Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HotelWizard API",
                    Version = "v1",
                    Description = "API для системы управления отелем"
                });

                // Опционально: добавьте поддержку авторизации (если нужно)
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            var supportedCultures = new[] {
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

            // Включаем Swagger UI
            app.UseSwagger();  // Генерация Swagger JSON
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelWizard API v1");
                options.RoutePrefix = string.Empty; // Устанавливаем Swagger UI по умолчанию на корень
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run();
        }
    }
}
