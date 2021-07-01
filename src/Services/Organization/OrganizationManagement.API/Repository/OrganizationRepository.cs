using OrganizationManagement.API.Models;

namespace OrganizationManagement.API.Repository
{
    public class OrganizationRepository : BaseRepo<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(AppDbContext context) : base(context)
        {
        }
    }
}