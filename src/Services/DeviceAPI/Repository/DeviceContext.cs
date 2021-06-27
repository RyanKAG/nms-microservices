using Microsoft.EntityFrameworkCore;
using DeviceAPI.Models;
namespace DeviceAPI.Repository
{
    public class DeviceContext : DbContext
    {
        public DeviceContext(DbContextOptions<DeviceContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        public DbSet<Device> Devices { get; set; }
    }
}