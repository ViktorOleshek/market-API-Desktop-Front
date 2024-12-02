using Abstraction.IEntities;
using Abstraction.IRepositories;
using DalMongoDB.Entities;
using DalMongoDB.Mappers;
using MongoDB.Driver;

namespace DalMongoDB.Repositories
{
    public class ProductRepository : AbstractRepository<IProduct>, IProductRepository
    {
        public ProductRepository(IMongoDatabase database)
            : base(database, "Products")
        {
            ArgumentNullException.ThrowIfNull(database);
        }

        public IProduct CreateEntity()
        {
            return new Product { Category = new ProductCategory() };
        }

        public async Task<IEnumerable<IProduct>> GetAllWithDetailsAsync()
        {
            var productCollection = this.Collection;
            var receiptDetailCollectionName = "ReceiptDetails";
            var categoryCollectionName = "Categories";

            var aggregation = await productCollection.Aggregate()
                .Lookup(
                    foreignCollectionName: receiptDetailCollectionName,
                    localField: "Id",
                    foreignField: "ProductId",
                    @as: "ReceiptDetails"
                )
                .Lookup(
                    foreignCollectionName: categoryCollectionName,
                    localField: "CategoryId",
                    foreignField: "Id",
                    @as: "CategoryDetails"
                )
                .ToListAsync();

            var result = new List<IProduct>();
            foreach (var item in aggregation)
            {
                var product = EntityMapper.MapToProduct(item);
                result.Add(product);
            }

            return result;
        }

        public async Task<IProduct> GetByIdWithDetailsAsync(int id)
        {
            var productCollection = this.Collection;
            var receiptDetailCollectionName = "ReceiptDetails";
            var categoryCollectionName = "Categories";

            var aggregation = await productCollection.Aggregate()
                .Match(p => p.Id == id)
                .Lookup(
                    foreignCollectionName: receiptDetailCollectionName,
                    localField: "Id",
                    foreignField: "ProductId",
                    @as: "ReceiptDetails"
                )
                .Lookup(
                    foreignCollectionName: categoryCollectionName,
                    localField: "CategoryId",
                    foreignField: "Id",
                    @as: "CategoryDetails"
                )
                .FirstOrDefaultAsync();

            return EntityMapper.MapToProduct(aggregation);
        }
    }
}
