using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

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

            // serve client and server together in production
            if (env.IsProduction())
            {
                services.AddSpaStaticFiles(o =>
                {
                    o.RootPath = "wwwroot";
                });
            }

            if (env.IsDevelopment())
            {
                services.AddCors();
            }

            // asp.net
            services.AddHttpContextAccessor();
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

                app.UseCors(o => o
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    //.AllowAnyOrigin()
                    .WithOrigins("http://localhost:3000")
                    .AllowCredentials()
                    .Build());
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseCookiePolicy();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // serve client and server together in production
            if (env.IsProduction())
            {
                app.UseSpaStaticFiles();
                app.UseSpa(o =>
                {
                    o.Options.SourcePath = "wwwroot";
                });
            }
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
            authOptions.ClientId = Environment.GetEnvironmentVariable("Auth_ClientId") ?? authOptions.ClientId;
            authOptions.ClientSecret = Environment.GetEnvironmentVariable("Auth_ClientSecret") ?? authOptions.ClientSecret;
            authOptions.ApiIdentifier = Environment.GetEnvironmentVariable("Auth_ApiIdentifier") ?? authOptions.ApiIdentifier;
            authOptions.TestingClientAccessToken = Environment.GetEnvironmentVariable("Auth_TestingClientAccessToken") ?? authOptions.TestingClientAccessToken;

            // register auth core
            services.AddSingleton(authOptions);
            services.AddSingleton<
                Auth0.ManagementApi.IManagementConnection,
                Auth0.ManagementApi.HttpClientManagementConnection>();
            services.AddSingleton<UtilityServices.AuthService>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            // add aspnet auth
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddOpenIdConnect("Auth0", options =>
            {
                //var appRootPath = env.IsProduction() ? "https://" + Environment.GetEnvironmentVariable("DEPLOCA_SERVICEURL_ui-and-api") : "";
                options.Authority = (!authOptions.ServerRootUrl.StartsWith("https") ? "https://" : "") + authOptions.ServerRootUrl;
                options.ClientId = authOptions.ClientId;
                options.ClientSecret = authOptions.ClientSecret;
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.Scope.Add("openid profile email");
                options.CallbackPath = "/auth/signin-callback";
                options.ClaimsIssuer = "Auth0";
                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProvider = context =>
                    {
                        context.ProtocolMessage.SetParameter("audience", authOptions.ApiIdentifier);
                        return Task.FromResult(0);
                    },
                    // handle the logout redirection
                    OnRedirectToIdentityProviderForSignOut = (context) =>
                    {
                        var logoutUri = $"https://{authOptions.ServerRootUrl}/v2/logout?client_id={authOptions.ClientId}";

                        var postLogoutUri = context.Properties.RedirectUri;
                        if (!string.IsNullOrEmpty(postLogoutUri))
                        {
                            if (postLogoutUri.StartsWith("/"))
                            {
                                // transform to absolute
                                var request = context.Request;
                                postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                            }
                            logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
                        }

                        context.Response.Redirect(logoutUri);
                        context.HandleResponse();

                        return Task.CompletedTask;
                    },
                };
            }).AddCookie(options =>
            {
                options.LoginPath = "/auth/login";
                options.LogoutPath = "/auth/logout";
                options.Events = new CookieAuthenticationEvents
                {
                    // Prevent redirect to login page for ajax calls to apis
                    OnRedirectToLogin = context =>
                    {
                        if (context.Request.Path.Value.StartsWith("/api"))
                        {
                            context.Response.StatusCode = 401;
                            return Task.FromResult(0);
                        }
                        context.Response.Redirect(context.RedirectUri);
                        return Task.FromResult(0);
                    },
                };
            });
            //.AddJwtBearer(options =>
            //{
            //    options.Authority = (!authOptions.ServerRootUrl.StartsWith("https") ? "https://" : "") + authOptions.ServerRootUrl;
            //    options.Audience = authOptions.UiApp_Identifier;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier
            //    };
            //});
        }
    }
}
