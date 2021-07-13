using System;
using Event.Messages.Events;
using Event.Messages.Events.DeviceEvents;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NetworkAPI.EventBusConsumer;
using NetworkAPI.Repository;
using NetworkAPI.Models;

namespace NetworkAPI
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
            
            services.AddMassTransit(config =>
            {

                config.AddConsumer<DeviceDeletedConsumer>();
                config.AddConsumer<DeviceCreatedConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    var username = Configuration["RabbitMQ:Username"];
                    var password = Configuration["RabbitMQ:Password"];
                    var portString = Configuration["RabbitMQ:Port"];
                    var port = portString.Length == 0 ? "" : ":" + portString;
                    var host = Configuration["RabbitMQ:Host"];
                    var connection = $"amqp://{username}:{password}@{host}{port}/";
                    Console.WriteLine(connection);
                    cfg.Host(connection);
                    cfg.ReceiveEndpoint(EventBusConstants.DeviceDeletedQueue,
                        c => c.ConfigureConsumer<DeviceDeletedConsumer>(ctx)
                    );
                    cfg.ReceiveEndpoint(EventBusConstants.DeviceCreatedQueue,
                        c => c.ConfigureConsumer<DeviceCreatedConsumer>(ctx)
                    );
                });
            });
            services.AddMassTransitHostedService();
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<INetworkRepository, NetworkRepository>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetworkAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetworkAPI v1"));
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
