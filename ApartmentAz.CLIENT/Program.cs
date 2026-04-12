using ApartmentAz.CLIENT.Helpers;
using ApartmentAz.CLIENT.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ApartmentAz.CLIENT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var apiBase = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7001";

            // ── HttpClient services ──────────────────────────────────────────
            builder.Services.AddHttpClient<ApiListingService>(c => c.BaseAddress = new Uri(apiBase));
            builder.Services.AddHttpClient<ApiLocationService>(c => c.BaseAddress = new Uri(apiBase));
            builder.Services.AddHttpClient<ApiFavoriteService>(c => c.BaseAddress = new Uri(apiBase));
            builder.Services.AddHttpClient<ApiAuthService>(c => c.BaseAddress = new Uri(apiBase));
            builder.Services.AddHttpClient<ApiAgencyService>(c => c.BaseAddress = new Uri(apiBase));
            builder.Services.AddHttpClient<ApiResidentialComplexService>(c => c.BaseAddress = new Uri(apiBase));
            builder.Services.AddHttpClient<ApiAdminService>(c => c.BaseAddress = new Uri(apiBase));
            builder.Services.AddHttpClient<ApiChatService>(c => c.BaseAddress = new Uri(apiBase));
            builder.Services.AddHttpClient<ClientTranslationService>(c => c.BaseAddress = new Uri("https://api.mymemory.translated.net/"));
            builder.Services.AddMemoryCache();

            // ── Cookie Authentication ────────────────────────────────────────
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.LogoutPath = "/Auth/Logout";
                });

            builder.Services.AddSingleton<UILocalizer>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
