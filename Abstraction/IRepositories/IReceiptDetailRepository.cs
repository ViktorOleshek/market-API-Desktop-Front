using Abstraction.Models;

namespace Abstraction.IRepositories
{
    public interface IReceiptDetailRepository : IRepository<ReceiptDetailModel>
    {
        Task<IEnumerable<ReceiptDetailModel>> GetAllWithDetailsAsync();
    }
}