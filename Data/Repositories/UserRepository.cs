using System.Threading.Tasks;
using Abstraction.IEntities;
using Abstraction.IRepositories;
using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserRepository : AbstractRepository<IUser>, IUserRepository
    {
        public UserRepository(TradeMarketDbContext context)
            : base(context)
        {
        }

        public override IUser CreateEntity()
        {
            return new User();
        }

        public async Task<IUser> GetByUsernameAsync(string username)
        {
            return await this.Context.Set<User>()
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
