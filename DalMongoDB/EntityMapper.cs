using Abstraction.IEntities;
using DalMongoDB.Entities;
using MongoDB.Bson;
using System.Linq;

namespace DalMongoDB.Mappers
{
    public static class EntityMapper
    {
        public static Customer MapToCustomer(BsonDocument bsonCustomer)
        {
            return new Customer
            {
                Id = bsonCustomer ["_id"].AsInt32,
                PersonId = bsonCustomer ["PersonId"].AsInt32,
                DiscountValue = bsonCustomer ["DiscountValue"].AsInt32,
                Receipts = bsonCustomer ["Receipts"].AsBsonArray.Select(r => new Receipt
                {
                    Id = r ["_id"].AsInt32,
                    CustomerId = r ["CustomerId"].AsInt32,
                    OperationDate = r ["OperationDate"].ToUniversalTime(),
                    IsCheckedOut = r ["IsCheckedOut"].AsBoolean
                }).ToList(),
                Person = bsonCustomer ["Person"].AsBsonArray.Select(p => new Person
                {
                    Id = p ["_id"].AsInt32,
                    Name = p ["Name"].AsString,
                    Surname = p ["Surname"].AsString,
                    BirthDate = p ["BirthDate"].ToUniversalTime()
                }).FirstOrDefault()
            };
        }

        public static Receipt MapToReceipt(BsonDocument document)
        {
            return new Receipt
            {
                Id = document ["_id"].AsInt32,
                CustomerId = document ["CustomerId"].AsInt32,
                OperationDate = document ["OperationDate"].ToUniversalTime(),
                IsCheckedOut = document ["IsCheckedOut"].AsBoolean,
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
                }).FirstOrDefault(),
                ReceiptDetails = document ["ReceiptDetails"].AsBsonArray.Select(detail => new ReceiptDetail
                {
                    Id = detail ["_id"].AsInt32,
                    ReceiptId = detail ["ReceiptId"].AsInt32,
                    ProductId = detail ["ProductId"].AsInt32,
                    DiscountUnitPrice = detail ["DiscountUnitPrice"].AsDecimal,
                    UnitPrice = detail ["UnitPrice"].AsDecimal,
                    Quantity = detail ["Quantity"].AsInt32
                }).ToList()
            };
        }

        // Мапінг динамічного результату агрегації в ReceiptDetail
        public static IReceiptDetail MapToReceiptDetail(dynamic item)
        {
            var receiptDetail = new ReceiptDetail
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ReceiptId = item.ReceiptId,
                Quantity = item.Quantity,
                DiscountUnitPrice = item.DiscountUnitPrice,
                UnitPrice = item.UnitPrice
            };

            // Мапінг зв'язаних продуктів
            if (item.ProductDetails != null && item.ProductDetails.Count > 0)
            {
                var productDetails = item.ProductDetails [0]; // Припускаємо, що один продукт відповідає
                receiptDetail.Product = new Product
                {
                    Id = productDetails.Id,
                    ProductName = productDetails.Name,
                    Price = productDetails.Price
                };
            }

            // Мапінг зв'язаного рахунку
            if (item.ReceiptDetails != null && item.ReceiptDetails.Count > 0)
            {
                var receiptDetails = item.ReceiptDetails [0]; // Припускаємо, що один рахунок відповідає
                receiptDetail.Receipt = new Receipt
                {
                    Id = receiptDetails.Id,
                    OperationDate = receiptDetails.OperationDate,
                    IsCheckedOut = receiptDetails.IsCheckedOut
                };
            }

            return receiptDetail;
        }

        // Мапінг динамічного результату агрегації в Product
        public static IProduct MapToProduct(dynamic item)
        {
            var product = new Product
            {
                Id = item.Id,
                ProductName = item.Name,
                Price = item.Price,
                ProductCategoryId = item.CategoryId
            };

            // Мапінг зв'язаних ReceiptDetails
            if (item.ReceiptDetails != null)
            {
                var receiptDetailsList = new List<ReceiptDetail>(); // Список ReceiptDetail
                foreach (var receiptDetail in item.ReceiptDetails)
                {
                    receiptDetailsList.Add(new ReceiptDetail
                    {
                        Id = receiptDetail.Id,
                        ProductId = receiptDetail.ProductId,
                        ReceiptId = receiptDetail.ReceiptId,
                        Quantity = receiptDetail.Quantity,
                        DiscountUnitPrice = receiptDetail.DiscountUnitPrice,
                        UnitPrice = receiptDetail.UnitPrice
                    });
                }
                product.ReceiptDetails = receiptDetailsList;  // Приведення до правильного типу
            }

            // Мапінг зв'язаних CategoryDetails
            if (item.CategoryDetails != null && item.CategoryDetails.Count > 0)
            {
                var category = item.CategoryDetails [0]; // Припускаємо, що один продукт має одну категорію
                product.Category = new ProductCategory
                {
                    Id = category.Id,
                    CategoryName = category.Name
                };
            }

            return product;
        }
    }
}
