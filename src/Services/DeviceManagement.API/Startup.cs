using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using DeviceManagement.API.Models;
using DeviceManagement.API.Repository;
using Newtonsoft.Json.Serialization;

namespace DeviceManagement.API
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
            services.AddDbContext<DeviceContext>(op =>
            {
                op.UseSqlServer(Configuration["ConnectionStrings:DbConnectionString"]);
            });
            
            services.AddAutoMapper(typeof(Startup));
            
            services.AddControllers().AddNewtonsoftJson(op =>
            {
                op.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeviceManagement.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.EnvironmentName == "local")
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeviceManagement.API v1"));
                PrepDb.PrepPopulation(app);
            }

            app.UseHttpsRedirection();

            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
