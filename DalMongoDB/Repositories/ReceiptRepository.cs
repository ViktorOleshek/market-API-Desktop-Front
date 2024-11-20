using Abstraction.IEntities;
using Abstraction.IRepositories;
using DalMongoDB.Mappers;
using MongoDB.Driver;

namespace DalMongoDB.Repositories
{
    public class ReceiptRepository : AbstractRepository<IReceipt>, IReceiptRepository
    {
        public ReceiptRepository(IMongoDatabase database)
            : base(database, "Receipts")
        {
            ArgumentNullException.ThrowIfNull(database);
        }

        public async Task<IEnumerable<IReceipt>> GetAllWithDetailsAsync()
        {
            var aggregation = await this.Collection.Aggregate()
                .Lookup(
                    foreignCollectionName: "ReceiptDetails",
                    localField: "Id",
                    foreignField: "ReceiptId",
                    @as: "ReceiptDetails"
                )
                .Lookup(
                    foreignCollectionName: "Products",
                    localField: "ReceiptDetails.ProductId",
                    foreignField: "Id",
                    @as: "ProductDetails"
                )
                .Lookup(
                    foreignCollectionName: "Categories",
                    localField: "ProductDetails.CategoryId",
                    foreignField: "Id",
                    @as: "CategoryDetails"
                )
                .Lookup(
                    foreignCollectionName: "Customers",
                    localField: "CustomerId",
                    foreignField: "Id",
                    @as: "CustomerDetails"
                )
                .Lookup(
                    foreignCollectionName: "Persons",
                    localField: "CustomerDetails.PersonId",
                    foreignField: "Id",
                    @as: "PersonDetails"
                )
                .ToListAsync();

            var receipts = aggregation.Select(EntityMapper.MapToReceipt).ToList();

            return receipts;
        }

        public async Task<IReceipt> GetByIdWithDetailsAsync(int id)
        {
            var aggregation = await this.Collection.Aggregate()
                .Match(r => r.Id == id)
                .Lookup(
                    foreignCollectionName: "ReceiptDetails",
                    localField: "Id",
                    foreignField: "ReceiptId",
                    @as: "ReceiptDetails"
                )
                .Lookup(
                    foreignCollectionName: "Products",
                    localField: "ReceiptDetails.ProductId",
                    foreignField: "Id",
                    @as: "ProductDetails"
                )
                .Lookup(
                    foreignCollectionName: "Categories",
                    localField: "ProductDetails.CategoryId",
                    foreignField: "Id",
                    @as: "CategoryDetails"
                )
                .Lookup(
                    foreignCollectionName: "Customers",
                    localField: "CustomerId",
                    foreignField: "Id",
                    @as: "CustomerDetails"
                )
                .Lookup(
                    foreignCollectionName: "Persons",
                    localField: "CustomerDetails.PersonId",
                    foreignField: "Id",
                    @as: "PersonDetails"
                )
                .FirstOrDefaultAsync();

            if (aggregation == null)
                return null;

            var receipt = EntityMapper.MapToReceipt(aggregation);

            return receipt;
        }
    }
}
