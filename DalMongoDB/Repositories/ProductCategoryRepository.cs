using Abstraction.IEntities;
using Abstraction.IRepositories;
using DalMongoDB.Entities;
using MongoDB.Driver;

namespace DalMongoDB.Repositories
{
    public class ProductCategoryRepository : AbstractRepository<IProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(IMongoDatabase database)
            : base(database, "ProductCategories")
        {
            ArgumentNullException.ThrowIfNull(database);
        }

        public IProductCategory CreateEntity()
        {
            return new ProductCategory();
        }
    }
}
