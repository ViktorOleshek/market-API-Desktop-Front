using System.Threading.Tasks;
using DalMongoDB.Repositories;
using Abstraction.IRepositories;
using MongoDB.Driver;

namespace DalMongoDB.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoDatabase database;

        public UnitOfWork(IMongoDatabase database)
        {
            this.database = database;

            this.CustomerRepository = new CustomerRepository(database);
            this.PersonRepository = new PersonRepository(database);
            this.ProductRepository = new ProductRepository(database);
            this.ProductCategoryRepository = new ProductCategoryRepository(database);
            this.ReceiptRepository = new ReceiptRepository(database);
            this.ReceiptDetailRepository = new ReceiptDetailRepository(database);
        }

        public ICustomerRepository CustomerRepository { get; }

        public IPersonRepository PersonRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IProductCategoryRepository ProductCategoryRepository { get; }

        public IReceiptRepository ReceiptRepository { get; }

        public IReceiptDetailRepository ReceiptDetailRepository { get; }

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }
    }
}
