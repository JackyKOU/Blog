using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using PiBlog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PiBlog.Interface;
using PiBlog.Service;
using PiBlog.Configuations;

namespace PiBlog
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
            services.AddControllers();
            services.AddDbContext<PiDbContext>(options=>{
                options.UseSqlite(AppSettings.SqliteConnectionString);
            });

            services.AddScoped<IPostService,PostService>();
            services.AddRouting(options=>
            {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = true;
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options=>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // 是否验证颁发者
                            ValidateIssuer = true,
                            // 是否验证访问群体
                            ValidateAudience = true,
                            // 是否验证生存期
                            ValidateLifetime = true,
                            // 验证Token的时间偏移量
                            ClockSkew = TimeSpan.FromSeconds(30),
                            // 是否验证安全密钥
                            ValidateIssuerSigningKey = true,
                            // 访问群体
                            ValidAudience = AppSettings.JWT.Domain,
                            // 颁发者
                            ValidIssuer = AppSettings.JWT.Domain,
                            // 安全密钥
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.JWT.SecurityKey))
                        };
                    });
            // 添加响应缓存
            services.AddResponseCaching();
            // MVC服务
            services.AddMvcCore(options =>
            {
                // 添加一条响应缓存的默认配置
                options.CacheProfiles.Add("default", new CacheProfile { Duration = 100 });
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);

            // Http请求
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
