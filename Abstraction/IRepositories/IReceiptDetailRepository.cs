using Abstraction.IEntities;

namespace Abstraction.IRepositories
{
    public interface IReceiptDetailRepository : IRepository<IReceiptDetail>
    {
        Task<IEnumerable<IReceiptDetail>> GetAllWithDetailsAsync();
    }
}