using Abstraction.IEntities;
using Abstraction.IRepositories;
using DalMongoDB.Mappers;
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
                    foreignCollectionName: "Persons",
                    localField: "PersonId",
                    foreignField: "_id",
                    @as: "Person")
                .Lookup(
                    foreignCollectionName: "Receipts",
                    localField: "_id",
                    foreignField: "CustomerId",
                    @as: "Receipts")
                .ToListAsync();

            var customerList = customers.Select(bsonCustomer => EntityMapper.MapToCustomer(bsonCustomer)).ToList();

            return customerList;
        }

        public async Task<ICustomer> GetByIdWithDetailsAsync(int id)
        {
            var pipeline = new[]
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

            var mappedCustomer = EntityMapper.MapToCustomer(results);

            return mappedCustomer;
        }
    }
}
