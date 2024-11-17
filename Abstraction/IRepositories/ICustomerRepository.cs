using Abstraction.IEntities;

namespace Abstraction.IRepositories
{
    public interface ICustomerRepository : IRepository<ICustomer>
    {
        Task<IEnumerable<ICustomer>> GetAllWithDetailsAsync();

        Task<ICustomer> GetByIdWithDetailsAsync(int id);
    }
}