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
            builder.Entity<NetworkDevice>().HasKey(e => new {e.NetworkId, e.DeviceId});
        }

        public DbSet<Network> Networks { get; set; }
    }
}