using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NetworkAPI.Repository;

namespace NetworkAPI.Models
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext appDbContext)
        {
            appDbContext.Database.Migrate();
            if (!appDbContext.Devices.Any())
            {
                // var fakeDevice = new Faker<Network>();
                // List<Network> devices = fakeDevice.Generate(55);
                // deviceContext.Devices.AddRange(devices);
                // deviceContext.SaveChanges();
            }
        }
    }
}