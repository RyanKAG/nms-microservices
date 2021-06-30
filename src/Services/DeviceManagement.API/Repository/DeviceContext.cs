using Microsoft.EntityFrameworkCore;
using DeviceManagement.API.Models;

namespace DeviceManagement.API.Repository
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
        public DbSet<Mobile> Mobiles { get; set; }
    }
}