// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.EntityFrameworkCore;
// using Auth.API.Repository;
// using Microsoft.AspNetCore.Identity;
//
// namespace Auth.API.Utils
// {
//     public static class PrepDb
//     {
//         public static void PrepPopulation(IApplicationBuilder app)
//         {
//             using (var serviceScope = app.ApplicationServices.CreateScope())
//             {
//                 SeedData(serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole<Guid>>());
//             }
//         }
//
//         private static void SeedData(RoleManager<IdentityRole<Guid>> roleManager)
//         {
//             roleManager.Database.Migrate();
//         }
//     }
// }