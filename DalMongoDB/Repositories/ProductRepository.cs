using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstraction.IEntities;
using Abstraction.IRepositories;
using DalMongoDB.Data;
using DalMongoDB.Entities;
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

        public async Task<IEnumerable<IProduct>> GetAllWithDetailsAsync()
        {
            var productCollection = this.Collection;
            var receiptDetailCollectionName = "ReceiptDetails";
            var categoryCollectionName = "Categories";

            var aggregation = await productCollection.Aggregate()
                .Lookup(
                    foreignCollectionName: receiptDetailCollectionName,
                    localField: "Id",  // Product's Id matches with ReceiptDetail's ProductId
                    foreignField: "ProductId", // Field in ReceiptDetail that references Product's Id
                    @as: "ReceiptDetails"
                )
                .Lookup(
                    foreignCollectionName: categoryCollectionName,
                    localField: "CategoryId", // Product's CategoryId matches Category's Id
                    foreignField: "Id", // Category's Id
                    @as: "CategoryDetails"
                )
                .ToListAsync();

            return (IEnumerable<IProduct>)aggregation;
        }

        public async Task<IProduct> GetByIdWithDetailsAsync(int id)
        {
            var productCollection = this.Collection;
            var receiptDetailCollectionName = "ReceiptDetails";
            var categoryCollectionName = "Categories";

            var aggregation = await productCollection.Aggregate()
                .Match(p => p.Id == id) // Match the product by Id
                .Lookup(
                    foreignCollectionName: receiptDetailCollectionName,
                    localField: "Id",  // Product's Id matches with ReceiptDetail's ProductId
                    foreignField: "ProductId", // Field in ReceiptDetail that references Product's Id
                    @as: "ReceiptDetails"
                )
                .Lookup(
                    foreignCollectionName: categoryCollectionName,
                    localField: "CategoryId", // Product's CategoryId matches Category's Id
                    foreignField: "Id", // Category's Id
                    @as: "CategoryDetails"
                )
                .FirstOrDefaultAsync(); // Only return the first match (since it's a single product)

            return (IProduct)aggregation;
        }
    }
}
