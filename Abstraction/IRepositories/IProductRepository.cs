using Abstraction.Models;

namespace Abstraction.IRepositories
{
    public interface IProductRepository : IRepository<ProductModel>
    {
        Task<IEnumerable<ProductModel>> GetAllWithDetailsAsync();

        Task<ProductModel> GetByIdWithDetailsAsync(int id);
    }
}