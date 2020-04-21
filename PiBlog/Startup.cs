using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PiBlog.Configuations;
using PiBlog.Infrastructure;
using PiBlog.Interface;
using PiBlog.Service;

namespace PiBlog {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {

            services.AddControllers ().AddNewtonsoftJson ();
            services.AddDbContext<PiDbContext> (options => {
                options.UseSqlite (AppSettings.SqliteConnectionString);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor> ();
            services.AddScoped<IPostService, PostService> ();

            services.AddAuthentication (opts => {
                opts.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie (options => {
                options.LoginPath = "/signin";
                options.LogoutPath = "/signout";
            }).AddJwtBearer (JwtBearerDefaults.AuthenticationScheme, options => {
                options.Audience = AppSettings.JWT.Audience;

                options.TokenValidationParameters = new TokenValidationParameters {
                    // The signing key must match!
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey (
                    Encoding.UTF8.GetBytes (AppSettings.JWT.SecurityKey)),

                    // Validate the JWT Issuer (iss) claim
                    ValidateIssuer = true,
                    ValidIssuer = AppSettings.JWT.Issuer,

                    // Validate the JWT Audience (aud) claim
                    ValidateAudience = true,
                    ValidAudience = AppSettings.JWT.Audience,

                    // Validate the token expiry
                    ValidateLifetime = true,

                    // If you want to allow a certain amount of clock drift, set that here
                    //ClockSkew = TimeSpan.Zero
                };
            }).AddGitHub (options => {
                options.ClientId = AppSettings.GitHub.Client_ID;
                options.ClientSecret = AppSettings.GitHub.Client_Secret;
                //options.CallbackPath = new PathString("~/signin-github");//same as github develop setting CallBackPath，default is /signin-github
                options.Scope.Add ("user:email");

            }).AddGoogle (options => {
                options.ClientId = AppSettings.Google.Client_ID;
                options.ClientSecret = AppSettings.Google.Client_Secret;
                //options.CallbackPath = new PathString("~/signin-google");//same as google develop setting CallBackPath，default is /signin-google
            });;
            // services.AddCors();

            services.AddResponseCaching();
            // MVC??
            services.AddMvcCore(options =>
            {
                // ?????????????
                options.CacheProfiles.Add("default", new CacheProfile { Duration = 100 });
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseHttpsRedirection ();

            app.UseRouting ();

            // app.UseCors (builder => {
            //     string[] withOrigins = Configuration.GetSection ("WithOrigins").Get<string[]> ();

            //     builder.AllowAnyHeader ().AllowAnyMethod ().AllowCredentials ().WithOrigins (withOrigins);
            // });

            app.UseAuthentication ();
            app.UseAuthorization ();
            app.UseResponseCaching();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}