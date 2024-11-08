using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities;

namespace Abstraction.IRepositories
{
    public interface IReceiptDetailRepository : IRepository<ReceiptDetail>
    {
        Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync();
    }
}
