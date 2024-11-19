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
    public class ReceiptRepository : AbstractRepository<IReceipt>, IReceiptRepository
    {
        public ReceiptRepository(IMongoDatabase database)
            : base(database, "Receipts")
        {
            ArgumentNullException.ThrowIfNull(database);
        }

        public async Task<IEnumerable<IReceipt>> GetAllWithDetailsAsync()
        {
            var receiptCollection = this.Collection;
            var receiptDetailCollectionName = "ReceiptDetails";
            var productCollectionName = "Products";
            var categoryCollectionName = "Categories";
            var customerCollectionName = "Customers";
            var personCollectionName = "Persons";

            var aggregation = await receiptCollection.Aggregate()
                .Lookup(
                    foreignCollectionName: receiptDetailCollectionName,
                    localField: "Id",  // Receipt's Id matches with ReceiptDetail's ReceiptId
                    foreignField: "ReceiptId", // Field in ReceiptDetail that matches Receipt's Id
                    @as: "ReceiptDetails"
                )
                .Lookup(
                    foreignCollectionName: productCollectionName,
                    localField: "ReceiptDetails.ProductId", // ReceiptDetail's ProductId matches Product's Id
                    foreignField: "Id", // Product's Id
                    @as: "ProductDetails"
                )
                .Lookup(
                    foreignCollectionName: categoryCollectionName,
                    localField: "ProductDetails.CategoryId", // Product's CategoryId matches Category's Id
                    foreignField: "Id", // Category's Id
                    @as: "CategoryDetails"
                )
                .Lookup(
                    foreignCollectionName: customerCollectionName,
                    localField: "CustomerId", // Receipt's CustomerId
                    foreignField: "Id", // Customer's Id
                    @as: "CustomerDetails"
                )
                .Lookup(
                    foreignCollectionName: personCollectionName,
                    localField: "CustomerDetails.PersonId", // Customer's PersonId
                    foreignField: "Id", // Person's Id
                    @as: "PersonDetails"
                )
                .ToListAsync();

            return (IEnumerable<IReceipt>)aggregation;
        }

        public async Task<IReceipt> GetByIdWithDetailsAsync(int id)
        {
            var receiptCollection = this.Collection;
            var receiptDetailCollectionName = "ReceiptDetails";
            var productCollectionName = "Products";
            var categoryCollectionName = "Categories";
            var customerCollectionName = "Customers";
            var personCollectionName = "Persons";

            var aggregation = await receiptCollection.Aggregate()
                .Match(r => r.Id == id) // Match the receipt by Id
                .Lookup(
                    foreignCollectionName: receiptDetailCollectionName,
                    localField: "Id",
                    foreignField: "ReceiptId",
                    @as: "ReceiptDetails"
                )
                .Lookup(
                    foreignCollectionName: productCollectionName,
                    localField: "ReceiptDetails.ProductId",
                    foreignField: "Id",
                    @as: "ProductDetails"
                )
                .Lookup(
                    foreignCollectionName: categoryCollectionName,
                    localField: "ProductDetails.CategoryId",
                    foreignField: "Id",
                    @as: "CategoryDetails"
                )
                .Lookup(
                    foreignCollectionName: customerCollectionName,
                    localField: "CustomerId",
                    foreignField: "Id",
                    @as: "CustomerDetails"
                )
                .Lookup(
                    foreignCollectionName: personCollectionName,
                    localField: "CustomerDetails.PersonId",
                    foreignField: "Id",
                    @as: "PersonDetails"
                )
                .FirstOrDefaultAsync(); // Only return the first match (since it's a single receipt)

            return (IReceipt)aggregation;
        }
    }
}
