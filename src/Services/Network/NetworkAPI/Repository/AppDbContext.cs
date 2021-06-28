using Microsoft.EntityFrameworkCore;
using NetworkAPI.Models;

namespace NetworkAPI.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        public DbSet<Network> Devices { get; set; }
    }
}