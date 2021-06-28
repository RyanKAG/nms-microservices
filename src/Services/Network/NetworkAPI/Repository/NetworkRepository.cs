using NetworkAPI.Models;

namespace NetworkAPI.Repository
{
    public class NetworkRepository : BaseRepo<Network>, INetworkRepository
    {
        public NetworkRepository(AppDbContext context) : base(context)
        {
        }
    }
}