using BaltBet.ReleaseService.Integration.Bitbucket;
using BaltBet.ReleaseService.Integration.Options;
using LibGit2Sharp;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace BaltBet.ReleaseService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication();

            builder.Services.AddAuthorization();

            builder.Services.AddRazorPages();

            builder.Services.Configure<BitbucketOptions>(builder.Configuration.GetSection(BitbucketOptions.Position));
            builder.Services.AddTransient<BitbucketApiService>();

            var bitbucketOptions = new BitbucketOptions();
            builder.Configuration.GetSection(BitbucketOptions.Position)
                .Bind(bitbucketOptions);

            builder.Services.AddHttpClient<BitbucketApiService>(client =>
            {
                client.BaseAddress = new Uri(bitbucketOptions.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(
                        Encoding.UTF8.GetBytes(
                            $"{bitbucketOptions.UserName}:{bitbucketOptions.Password}")
                        )
                    );

                client.DefaultRequestHeaders.Add("X-Atlassian-Token", "nocheck");
            });

            builder.Services.AddCors();
            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //    app.MapControllers().AllowAnonymous();
            //else
            //    app.MapControllers();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();

                app.UseSpa(spa => { spa.UseProxyToSpaDevelopmentServer("http://localhost:3000"); });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .WithHeaders(HeaderNames.ContentType);
            });

            app.Run();
        }
    }
}