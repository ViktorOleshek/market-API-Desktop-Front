using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities;

namespace Abstraction.IRepositories
{
    public interface IReceiptRepository : IRepository<Receipt>
    {
        Task<IEnumerable<Receipt>> GetAllWithDetailsAsync();

        Task<Receipt> GetByIdWithDetailsAsync(int id);
    }
}
