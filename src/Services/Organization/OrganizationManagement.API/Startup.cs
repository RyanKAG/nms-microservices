using Event.Messages.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.API.EventBusConsumer;
using OrganizationManagement.API.Repository;

namespace OrganizationManagement.API
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

            services.AddDbContext<AppDbContext>(op =>
            {
                op.UseSqlServer(Configuration["ConnectionStrings:DbConnectionString"]);
            });
            
            services.AddAutoMapper(typeof(Startup));
            
            services.AddControllers();

            services.AddMassTransit(config =>
            {

                config.AddConsumer<NetworkCreatedConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    var username = Configuration["RabbitMQ:Username"];
                    var password = Configuration["RabbitMQ:Password"];
                    var portString = Configuration["RabbitMQ:Port"];
                    var port = portString.Length == 0 ? "" : ":" + portString;
                    var host = Configuration["RabbitMQ:Host"];
                    var connection = $"amqp://{username}:{password}@{host}{port}/";
                    cfg.Host(connection);
                    cfg.ReceiveEndpoint(EventBusConstants.NetworkCreatedQueue,
                        c => c.ConfigureConsumer<NetworkCreatedConsumer>(ctx)
                    );
                });
            });
            services.AddMassTransitHostedService();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrganizationManagement.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrganizationManagement.API v1"));
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
