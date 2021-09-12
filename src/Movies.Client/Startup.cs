using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Movies.Client.ApiServices;
using Movies.Client.HttpHandlers;
using System;

namespace Movies.Client
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
            services.AddControllersWithViews();

            services.AddScoped<IMovieApiService, MovieApiService>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                //options.AccessDeniedPath = "/Account/AccessDenied";
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://localhost:5005";
                options.AccessDeniedPath = "/Account/AccessDenied";

                options.ClientId = "movies_mvc_client";
                options.ClientSecret = "secret";
                //options.ResponseType = "code"; // use authentication flow
                options.ResponseType = "code id_token"; // use hybrid flow

                // openid and profile is not mandatory, automatically add by libraries.
                options.Scope.Add("openid");
                options.Scope.Add("profile");

                options.Scope.Add("movieAPI"); // Add this scope when using hybrid flow

                options.Scope.Add("address");
                options.Scope.Add("email");
                options.Scope.Add("roles");

                options.ClaimActions.MapUniqueJsonKey(claimType: "role", jsonKey: "role");
                options.ClaimActions.MapUniqueJsonKey(claimType: "address", jsonKey: "address");

                options.SaveTokens = true; // Store the token after successful authorization

                options.GetClaimsFromUserInfoEndpoint = true;

                // Requires token should have given name and role claims
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.GivenName,
                    RoleClaimType = JwtClaimTypes.Role
                };
            });

            // 1 create an HttpClient used for accessing the Movie.API
            services.AddTransient<AuthenticationDelegatingHandler>();

            services.AddHttpClient("MovieAPIClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            }).AddHttpMessageHandler<AuthenticationDelegatingHandler>();

            // 2 create an HttpClient used for accessing the IDP
            services.AddHttpClient("IDPClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:5005");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            });

            services.AddHttpContextAccessor();

            //services.AddSingleton(new ClientCredentialsTokenRequest
            //{
            //    Address = "https://localhost:5005/connect/token",
            //    ClientId = "movieClient",
            //    ClientSecret = "secret",
            //    Scope = "movieAPI"
            //});
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