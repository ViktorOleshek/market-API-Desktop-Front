using Abstraction.Entities;

namespace Abstraction.IRepositories;

public interface IReceiptDetailRepository
    : IRepository<ReceiptDetail>
{
    Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync();
}
