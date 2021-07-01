using Microsoft.EntityFrameworkCore;
using OrganizationManagement.API.Models;

namespace OrganizationManagement.API.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        public DbSet<Organization> Organizations { get; set; }
    }
}