using Abstraction.Models;

namespace Abstraction.IRepositories
{
    public interface ICustomerRepository : IRepository<CustomerModel>
    {
        Task<IEnumerable<CustomerModel>> GetAllWithDetailsAsync();

        Task<CustomerModel> GetByIdWithDetailsAsync(int id);
    }
}