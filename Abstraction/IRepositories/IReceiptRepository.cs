using Abstraction.Entities;

namespace Abstraction.IRepositories;

public interface IReceiptRepository
    : IRepository<Receipt>
{
    Task<IEnumerable<Receipt>> GetReceiptsByCustomerIdAsync(int customerId);

    Task<IEnumerable<Receipt>> GetAllWithDetailsAsync();

    Task<Receipt> GetByIdWithDetailsAsync(int id);
}
