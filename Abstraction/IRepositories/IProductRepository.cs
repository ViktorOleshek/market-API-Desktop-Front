using Abstraction.Entities;

namespace Abstraction.IRepositories;

public interface IProductRepository
    : IRepository<Product>
{
    Task<IEnumerable<Product>> GetAllWithDetailsAsync();

    Task<Product> GetByIdWithDetailsAsync(int id);
}
