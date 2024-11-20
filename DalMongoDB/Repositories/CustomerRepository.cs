using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstraction.IEntities;
using Abstraction.IRepositories;
using DalMongoDB.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DalMongoDB.Repositories
{
    public class CustomerRepository : AbstractRepository<ICustomer>, ICustomerRepository
    {
        public CustomerRepository(IMongoDatabase database)
            : base(database, "Customers")
        {
            ArgumentNullException.ThrowIfNull(database);
        }

        public async Task<IEnumerable<ICustomer>> GetAllWithDetailsAsync()
        {
            var customers = await this.Collection
                .Aggregate()
                .Lookup(
                    foreignCollectionName: "Persons",  // Зв'язана колекція
                    localField: "PersonId",            // Поле у Customer
                    foreignField: "_id",               // Поле у Persons
                    @as: "Person")
                .Lookup(
                    foreignCollectionName: "Receipts",
                    localField: "_id",
                    foreignField: "CustomerId",
                    @as: "Receipts")
                .ToListAsync();

            var customerList = new List<ICustomer>();

            foreach (var bsonCustomer in customers)
            {
                var mappedCustomer = new Customer
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

                customerList.Add(mappedCustomer);
            }

            return customerList;
        }

        public async Task<ICustomer> GetByIdWithDetailsAsync(int id)
        {
            // Агрегація через поточну колекцію
            var pipeline = new []
            {
        new BsonDocument("$match", new BsonDocument("_id", id)),
        new BsonDocument("$lookup", new BsonDocument
        {
            { "from", "Persons" },
            { "localField", "PersonId" },
            { "foreignField", "_id" },
            { "as", "Person" }
        }),
        new BsonDocument("$lookup", new BsonDocument
        {
            { "from", "Receipts" },
            { "localField", "_id" },
            { "foreignField", "CustomerId" },
            { "as", "Receipts" }
        })
    };

            var results = await this.Collection.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();

            if (results == null)
            {
                return null;
            }

            // Мапінг результатів
            var mappedCustomer = new Customer
            {
                Id = results ["_id"].AsInt32,
                PersonId = results ["PersonId"].AsInt32,
                DiscountValue = results ["DiscountValue"].AsInt32,
                Receipts = results ["Receipts"].AsBsonArray.Select(r => new Receipt
                {
                    Id = r ["_id"].AsInt32,
                    CustomerId = r ["CustomerId"].AsInt32,
                    OperationDate = r ["OperationDate"].ToUniversalTime(),
                    IsCheckedOut = r ["IsCheckedOut"].AsBoolean
                }).ToList(),
                Person = results ["Person"].AsBsonArray.Select(p => new Person
                {
                    Id = p ["_id"].AsInt32,
                    Name = p ["Name"].AsString,
                    Surname = p ["Surname"].AsString,
                    BirthDate = p ["BirthDate"].ToUniversalTime()
                }).FirstOrDefault()
            };

            return mappedCustomer;
        }
    }
}
