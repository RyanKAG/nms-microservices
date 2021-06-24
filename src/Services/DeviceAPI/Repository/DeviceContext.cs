using Microsoft.EntityFrameworkCore;
using DeviceAPI.Models;
namespace DeviceAPI.Repository
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        public DbSet<Device> Albums { get; set; }
    }
}