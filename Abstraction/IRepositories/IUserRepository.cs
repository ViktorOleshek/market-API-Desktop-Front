using Abstraction.IEntities;

namespace Abstraction.IRepositories
{
    public interface IUserRepository : IRepository<IUser>
    {
        Task<IUser> GetByUsernameAsync(string username);
    }
}
