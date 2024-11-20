using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstraction.IEntities;
using Abstraction.IRepositories;
using DalMongoDB.Data;
using DalMongoDB.Entities;
using MongoDB.Bson;
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

            // Якщо агрегація повертає порожній список, повертаємо порожній результат
            if (aggregation == null)
                return new List<IReceipt>();

            // Мапінг результатів агрегації
            var receipts = aggregation.Select(document =>
            {
                var receipt = new Receipt
                {
                    Id = document ["_id"].AsInt32,
                    CustomerId = document ["CustomerId"].AsInt32,
                    OperationDate = document ["OperationDate"].ToUniversalTime(),
                    IsCheckedOut = document ["IsCheckedOut"].AsBoolean,
                    Customer = document ["CustomerDetails"].AsBsonArray
                        .Select(customer => new Customer
                        {
                            Id = customer ["_id"].AsInt32,
                            PersonId = customer ["PersonId"].AsInt32,
                            DiscountValue = customer ["DiscountValue"].AsInt32,
                            Person = customer ["PersonDetails"].AsBsonArray
                                .Select(person => new Person
                                {
                                    Id = person ["_id"].AsInt32,
                                    Name = person ["FirstName"].AsString,
                                    Surname = person ["LastName"].AsString,
                                    // Інші поля для Person, якщо потрібно
                                }).FirstOrDefault()
                        }).FirstOrDefault(),
                    ReceiptDetails = document ["ReceiptDetails"].AsBsonArray
                        .Select(detail => new ReceiptDetail
                        {
                            Id = detail ["_id"].AsInt32,
                            ReceiptId = detail ["ReceiptId"].AsInt32,
                            ProductId = detail ["ProductId"].AsInt32,
                            DiscountUnitPrice = detail ["DiscountUnitPrice"].AsDecimal,
                            UnitPrice = detail ["UnitPrice"].AsDecimal,
                            Quantity = detail ["Quantity"].AsInt32
                        }).ToList()
                };

                return receipt;
            }).ToList();

            return receipts;
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

            // Якщо не знайдено жодного запису, повертаємо null
            if (aggregation == null)
                return null;

            // Створюємо Receipt з отриманих даних
            var document = aggregation;

            var receipt = new Receipt
            {
                Id = document ["_id"].AsInt32,
                CustomerId = document ["CustomerId"].AsInt32,
                OperationDate = document ["OperationDate"].ToUniversalTime(),
                IsCheckedOut = document ["IsCheckedOut"].AsBoolean,
                Customer = document ["CustomerDetails"].AsBsonArray
                    .Select(customer => new Customer
                    {
                        Id = customer ["_id"].AsInt32,
                        PersonId = customer ["PersonId"].AsInt32,
                        DiscountValue = customer ["DiscountValue"].AsInt32,
                        Person = new Person
                        {
                            Id = customer ["PersonDetails"] [0] ["_id"].AsInt32,
                            Name = customer ["PersonDetails"] [0] ["Name"].AsString,
                            Surname = customer ["PersonDetails"] [0] ["Surname"].AsString
                            // Додати інші поля, якщо потрібно
                        }
                    }).FirstOrDefault(),
                ReceiptDetails = document ["ReceiptDetails"].AsBsonArray
                    .Select(detail => new ReceiptDetail
                    {
                        Id = detail ["_id"].AsInt32,
                        ReceiptId = detail ["ReceiptId"].AsInt32,
                        ProductId = detail ["ProductId"].AsInt32,
                        DiscountUnitPrice = detail ["DiscountUnitPrice"].AsDecimal,
                        UnitPrice = detail ["UnitPrice"].AsDecimal,
                        Quantity = detail ["Quantity"].AsInt32
                    }).ToList()
            };

            return receipt;
        }

        private IReceipt MapToReceipt(BsonDocument document)
        {
            return new Receipt
            {
                Id = document ["_id"].AsInt32,
                CustomerId = document ["CustomerId"].AsInt32,
                OperationDate = document ["OperationDate"].ToUniversalTime(),
                IsCheckedOut = document ["IsCheckedOut"].AsBoolean,
                ReceiptDetails = document ["ReceiptDetails"].AsBsonArray.Select(detail => new ReceiptDetail
                {
                    Id = detail ["_id"].AsInt32,
                    ReceiptId = detail ["ReceiptId"].AsInt32,
                    ProductId = detail ["ProductId"].AsInt32,
                    DiscountUnitPrice = detail ["DiscountUnitPrice"].AsDecimal,
                    UnitPrice = detail ["UnitPrice"].AsDecimal,
                    Quantity = detail ["Quantity"].AsInt32
                }).ToList(),
                Customer = document ["CustomerDetails"].AsBsonArray.Select(c => new Customer
                {
                    Id = c ["_id"].AsInt32,
                    PersonId = c ["PersonId"].AsInt32,
                    DiscountValue = c ["DiscountValue"].AsInt32,
                    Person = c ["PersonDetails"].AsBsonArray.Select(p => new Person
                    {
                        Id = p ["_id"].AsInt32,
                        Name = p ["Name"].AsString,
                        Surname = p ["Surname"].AsString,
                        BirthDate = p ["BirthDate"].ToUniversalTime()
                    }).FirstOrDefault()
                }).FirstOrDefault()
            };
        }

    }
}
