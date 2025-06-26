using Abstraction.Entities;
using Abstraction.IRepositories;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Data.Repositories;

public class UserRepository
    : AbstractRepository<User>, IUserRepository
{
    public UserRepository(TradeMarketDbContext context)
        : base(context)
    {
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        return await this.Context.Set<User>()
            .Include(u => u.Person)
            .FirstOrDefaultAsync(u => u.Username == username);
    }
    public async Task<User> GetByEmailAsync(string email)
    {
        return await this.Context.Set<User>()
            .Include(u => u.Person)
            .FirstOrDefaultAsync(u => u.Username == email || u.Email == email);
    }
}
