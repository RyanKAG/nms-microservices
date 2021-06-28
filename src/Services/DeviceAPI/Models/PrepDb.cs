using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DeviceAPI.Repository;

namespace DeviceAPI.Models
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<DeviceContext>());
            }
        }

        private static void SeedData(DeviceContext deviceContext)
        {
            deviceContext.Database.Migrate();
            if (!deviceContext.Devices.Any())
            {
                var fakeDevice = new Faker<Device>()
                    .RuleFor(p => p.Id, s => Guid.NewGuid())
                    .RuleFor(p => p.Name, f => f.Commerce.Product())
                    .RuleFor(p => p.UpdatedOn, f => DateTime.UtcNow)
                    .RuleFor(p => p.Ip, f => f.Internet.Ip())
                    .RuleFor(p => p.MacAddress, f => f.Internet.Mac())
                    .RuleFor(p => p.IsBlocked, f => f.Random.Bool())
                    .RuleFor(p => p.LastUsedDate, f => f.Date.Past());

                List<Device> devices = fakeDevice.Generate(55);
                deviceContext.Devices.AddRange(devices);
                deviceContext.SaveChanges();
            }
        }
    }
}