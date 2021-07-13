using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetworkAPI.Models;

namespace NetworkAPI.Repository
{
    public class NetworkRepository : BaseRepo<Network>, INetworkRepository
    {
        public NetworkRepository(AppDbContext context) : base(context)
        {
        }

        
        public override async Task<Network> GetByIdAsync(Guid id)
        {
            var network = await Context.Networks.FirstOrDefaultAsync(p => p.Id == id);

            if (network == null)
                return null;

            // var devices = await Context.NetworkDevice
            //     .Where(n => n.NetworkId == id)
            //     .Select(n => n.DeviceId)
            //     .ToListAsync();
            // network.ConnectedDevices = devices;

            return network;
        }

        public Task<Device> GetDeviceById(Guid id)
        {
            return Context.Devices.FirstOrDefaultAsync(device => device.Id == id);
        }

        public async Task AddDevice(Device device)
        {
            await Context.Devices.AddAsync(device);
        }

        public void ConnectDevice(Network network, Device device)
        {
            device.Networks.Add(network);
        }
    }
}