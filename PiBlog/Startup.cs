using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PiBlog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PiBlog.Interface;
using PiBlog.Service;
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
                var connectionString = Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlite(connectionString);
            });

            services.AddScoped<IPostService,PostService>();

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
