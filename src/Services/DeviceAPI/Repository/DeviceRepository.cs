using DeviceAPI.Models;

namespace DeviceAPI.Repository
{
    public class DeviceRepository : BaseRepo<Device>, IDeviceRepository
    {
        public DeviceRepository(DeviceContext context) : base(context)
        {
        }
    }
}