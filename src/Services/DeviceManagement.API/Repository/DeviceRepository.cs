using DeviceManagement.API.Models;

namespace DeviceManagement.API.Repository
{
    public class DeviceRepository : BaseRepo<Device>, IDeviceRepository
    {
        public DeviceRepository(DeviceContext context) : base(context)
        {
        }
    }
}