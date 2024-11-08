using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities;

namespace Abstraction.IRepositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetAllWithDetailsAsync();

        Task<Customer> GetByIdWithDetailsAsync(int id);
    }
}
