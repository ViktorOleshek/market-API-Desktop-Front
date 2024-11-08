using Abstraction.Models;

namespace Abstraction.IRepositories
{
    public interface IReceiptRepository : IRepository<ReceiptModel>
    {
        Task<IEnumerable<ReceiptModel>> GetAllWithDetailsAsync();

        Task<ReceiptModel> GetByIdWithDetailsAsync(int id);
    }
}