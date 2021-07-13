using System;
using System.Threading.Tasks;
using NetworkAPI.Models;

namespace NetworkAPI.Repository
{
    public interface INetworkRepository : IRepository<Network>
    {
        public Task AddDevice(Device device);
        public Task<Device> GetDeviceById(Guid id);

        public void ConnectDevice(Network network, Device device);
    }
}