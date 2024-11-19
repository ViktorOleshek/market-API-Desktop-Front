using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstraction.IEntities;
using Abstraction.IRepositories;
using DalMongoDB.Entities;
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
                    foreignCollectionName: "Persons", // Зв'язана колекція
                    localField: "PersonId",           // Поле у Customer
                    foreignField: "_id",              // Поле у Persons
                    @as: "Person")                    // Ім'я для збереження результату
                .Lookup(
                    foreignCollectionName: "Receipts",
                    localField: "_id",
                    foreignField: "CustomerId",
                    @as: "Receipts")
                .ToListAsync();

            return (IEnumerable<ICustomer>)customers;
        }

        public async Task<ICustomer> GetByIdWithDetailsAsync(int id)
        {
            var customer = await this.Collection
                .Aggregate()
                .Match(x => x.Id == id)
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
                .FirstOrDefaultAsync();

            return (ICustomer)customer;
        }
    }
}
