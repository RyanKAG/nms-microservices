namespace Auth.API.Repository
{
    public class RoleRepository : BaseRepo<Models.Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }
    }
}