using Abstraction.IEntities;
using Abstraction.IRepositories;
using MongoDB.Driver;
using DalMongoDB.Mappers;

namespace DalMongoDB.Repositories
{
    public class ReceiptDetailRepository : AbstractRepository<IReceiptDetail>, IReceiptDetailRepository
    {
        public ReceiptDetailRepository(IMongoDatabase database)
            : base(database, "ReceiptDetails")
        {
            ArgumentNullException.ThrowIfNull(database);
        }

        public async Task<IEnumerable<IReceiptDetail>> GetAllWithDetailsAsync()
        {
            var receiptDetailCollection = this.Collection;
            var productCollectionName = "Products";
            var receiptCollectionName = "Receipts";

            var aggregation = await receiptDetailCollection.Aggregate()
                .Lookup(
                    foreignCollectionName: productCollectionName,
                    localField: "ProductId",
                    foreignField: "Id",
                    @as: "ProductDetails"
                )
                .Lookup(
                    foreignCollectionName: receiptCollectionName,
                    localField: "ReceiptId",
                    foreignField: "Id",
                    @as: "ReceiptDetails" 
                )
                .ToListAsync();

            var result = new List<IReceiptDetail>();
            foreach (var item in aggregation)
            {
                var receiptDetail = EntityMapper.MapToReceiptDetail(item);
                result.Add(receiptDetail);
            }

            return result;
        }
    }
}
