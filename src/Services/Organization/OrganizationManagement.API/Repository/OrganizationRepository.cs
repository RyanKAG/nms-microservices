using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.API.Models;
using System.Linq;
namespace OrganizationManagement.API.Repository
{
    public class OrganizationRepository : BaseRepo<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Network> GetNetworkById(Guid id)
        {
           return await Context.Networks.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async void AddNetwork(Network network)
        {
             await Context.Networks.AddAsync(network);
        }

        public void AddNetworkToOrg(Organization org, Network network)
        {
            network.Organization = org;
        }
        
        
    }
}