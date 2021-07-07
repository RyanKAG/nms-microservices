using Auth.API.Models;

namespace Auth.API.Repository
{
    public class UserRepository : BaseRepo<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }
    }
}