using Abstraction.IEntities;

namespace Abstraction.IRepositories
{
    public interface IProductRepository : IRepository<IProduct>
    {
        Task<IEnumerable<IProduct>> GetAllWithDetailsAsync();

        Task<IProduct> GetByIdWithDetailsAsync(int id);
    }
}