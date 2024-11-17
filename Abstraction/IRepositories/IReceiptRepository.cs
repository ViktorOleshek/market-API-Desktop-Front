using Abstraction.IEntities;

namespace Abstraction.IRepositories
{
    public interface IReceiptRepository : IRepository<IReceipt>
    {
        Task<IEnumerable<IReceipt>> GetAllWithDetailsAsync();

        Task<IReceipt> GetByIdWithDetailsAsync(int id);
    }
}