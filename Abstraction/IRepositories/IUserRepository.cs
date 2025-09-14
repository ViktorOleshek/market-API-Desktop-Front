using Abstraction.Entities;

namespace Abstraction.IRepositories;

public interface IUserRepository
    : IRepository<User>
{
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByEmailAsync(string email);
}
