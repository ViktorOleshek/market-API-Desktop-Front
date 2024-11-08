using System.Collections.Generic;
using System.Threading.Tasks;
using Abstraction.Models;

namespace Abstraction.IServices
{
    public interface ICustomerService : ICrud<CustomerModel>
    {
        Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId);
    }
}
