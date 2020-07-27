using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Knowledgebase.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IWebHostEnvironment env { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // core
            RegisterDatabase(services);
            RegisterDataServices(services);
            RegisterUtilityServices(services);

            // authentication
            RegisterAuthentication(services);

            // asp.net
            services.AddHttpContextAccessor();
            services.AddCors();
            services.AddControllers()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add<Filters.ExceptionActionFilter>();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(o => o
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .Build());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private void RegisterDatabase(IServiceCollection services)
        {
            services.AddDbContext<Persistent.AppDbContext>(o =>
                o.UseInMemoryDatabase("Knowledgebase"));
            //o.UseNpgsql("Host=localhost;Database=Deploca.Knowledgebase;Username=postgres;Password=123456", pgOptions =>
            //    pgOptions.MigrationsAssembly(typeof(Persistent.AppDbContext).Assembly.GetName().Name)));
            services.AddScoped<UnitOfWork.IUnitOfWork>(provider =>
                provider.GetRequiredService<Persistent.AppDbContext>());
        }

        private void RegisterDataServices(IServiceCollection services)
        {
            services.AddTransient<Application.Services.AdministrationService>();
            services.AddTransient<Application.Services.AppSettingService>();
            services.AddTransient<Application.Services.AppUserService>();
            services.AddTransient<Application.Services.CategoryService>();
            services.AddTransient<Application.Services.ThreadService>();
        }

        private void RegisterUtilityServices(IServiceCollection services)
        {
            services.AddScoped<UtilityServices.IAppSession, Utilities.AppSession>();
        }

        private void RegisterAuthentication(IServiceCollection services)
        {
            // load configuration from env or appsettings
            var authOptions = Configuration.GetSection("Auth").Get<UtilityServices.AuthOptions>();
            authOptions.ServerRootUrl = Environment.GetEnvironmentVariable("Auth_ServerRootUrl") ?? authOptions.ServerRootUrl;
            authOptions.Management_ClientId = Environment.GetEnvironmentVariable("Auth_Management_ClientId") ?? authOptions.Management_ClientId;
            authOptions.Management_ClientSecret = Environment.GetEnvironmentVariable("Auth_Management_ClientSecret") ?? authOptions.Management_ClientSecret;
            authOptions.Management_Identifier = Environment.GetEnvironmentVariable("Auth_Management_Identifier") ?? authOptions.Management_Identifier;
            authOptions.Management_TestingClientAccessToken = Environment.GetEnvironmentVariable("Auth_Management_TestingClientAccessToken") ?? authOptions.Management_TestingClientAccessToken;
            authOptions.UiApp_ClientId = Environment.GetEnvironmentVariable("Auth_UiApp_ClientId") ?? authOptions.UiApp_ClientId;
            authOptions.UiApp_ClientSecret = Environment.GetEnvironmentVariable("Auth_UiApp_ClientSecret") ?? authOptions.UiApp_ClientSecret;
            authOptions.UiApp_Identifier = Environment.GetEnvironmentVariable("Auth_UiApp_Identifier") ?? authOptions.UiApp_Identifier;

            // register auth core
            services.AddSingleton(authOptions);
            services.AddSingleton<
                Auth0.ManagementApi.IManagementConnection,
                Auth0.ManagementApi.HttpClientManagementConnection>();
            services.AddSingleton<UtilityServices.AuthService>();

            // add aspnet auth
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = (!authOptions.ServerRootUrl.StartsWith("https") ? "https://" : "") + authOptions.ServerRootUrl;
                options.Audience = authOptions.UiApp_Identifier;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier
                };
            });
        }
    }
}
