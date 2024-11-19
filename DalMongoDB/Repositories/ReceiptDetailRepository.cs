using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstraction.IEntities;
using Abstraction.IRepositories;
using MongoDB.Driver;
using DalMongoDB.Entities;

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
            var productCollectionName = "Products"; // Collection name as a string
            var receiptCollectionName = "Receipts"; // Collection name as a string

            var aggregation = await receiptDetailCollection.Aggregate()
                .Lookup(
                    foreignCollectionName: productCollectionName, // Name of the "foreign" collection
                    localField: "ProductId", // Field in ReceiptDetail to match
                    foreignField: "Id", // Field in Product to match
                    @as: "ProductDetails" // Output alias
                )
                .Lookup(
                    foreignCollectionName: receiptCollectionName, // Name of the "foreign" collection
                    localField: "ReceiptId", // Field in ReceiptDetail to match
                    foreignField: "Id", // Field in Receipt to match
                    @as: "ReceiptDetails" // Output alias
                )
                .ToListAsync();

            return (IEnumerable<IReceiptDetail>)aggregation;
        }

    }
}
