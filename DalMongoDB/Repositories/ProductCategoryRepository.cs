using Abstraction.Entities;
using Abstraction.IRepositories;
using MongoDB.Driver;

namespace DalMongoDB.Repositories;

public class ProductCategoryRepository
    : AbstractRepository<ProductCategory>, IProductCategoryRepository
{
    public ProductCategoryRepository(IMongoDatabase database)
        : base(database, "ProductCategories")
    {
        ArgumentNullException.ThrowIfNull(database);
    }
}
