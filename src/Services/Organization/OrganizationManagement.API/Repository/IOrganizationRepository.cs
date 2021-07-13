using System;
using System.Threading.Tasks;
using OrganizationManagement.API.Models;

namespace OrganizationManagement.API.Repository
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        public void AddNetwork(Network network);
        public Task<Network> GetNetworkById(Guid id);
        public void AddNetworkToOrg(Organization org, Network network);
    }
}