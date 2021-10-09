using System;
using _Framework.Auth;
using CoreLayer.AccountAgg.Contract;
using CoreLayer.AccountAgg.Services;
using CoreLayer.ChatAgg.Contract;
using CoreLayer.ChatAgg.Services;
using DataLayer;
using DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerHost.Framework.Auth;
using ServerHost.Hubs.Chat;

namespace ServerHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MeMessengerContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,options =>
                {
                    options.LoginPath = "/auth";
                    options.LogoutPath = "/auth?handler=signout";
                    options.ExpireTimeSpan = TimeSpan.FromDays(5);
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AccessCors", builder => builder
                    .WithOrigins("http://localhost:5000")
                    .AllowAnyHeader()
                    .AllowAnyHeader()
                    .Build()
                );
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountServices, AccountServices>();
            services.AddScoped<IChatServices, ChatServices>();
            services.AddSingleton<IAuthHelper, AuthHelper>();

            services.AddSignalR();
            services.AddHttpContextAccessor();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AccessCors");
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}