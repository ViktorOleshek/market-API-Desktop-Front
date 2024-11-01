using System;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TradeMarketDbContext dbContext;

        public UnitOfWork(TradeMarketDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.CustomerRepository = new CustomerRepository(dbContext);
            this.PersonRepository = new PersonRepository(dbContext);
            this.ProductRepository = new ProductRepository(dbContext);
            this.ProductCategoryRepository = new ProductCategoryRepository(dbContext);
            this.ReceiptRepository = new ReceiptRepository(dbContext);
            this.ReceiptDetailRepository = new ReceiptDetailRepository(dbContext);
        }

        public UnitOfWork(
            TradeMarketDbContext dbContext,
            ICustomerRepository customerRepository,
            IPersonRepository personRepository,
            IProductRepository productRepository,
            IProductCategoryRepository productCategoryRepository,
            IReceiptRepository receiptRepository,
            IReceiptDetailRepository receiptDetailRepository)
        {
            this.dbContext = dbContext;
            this.CustomerRepository = customerRepository;
            this.PersonRepository = personRepository;
            this.ProductRepository = productRepository;
            this.ProductCategoryRepository = productCategoryRepository;
            this.ReceiptRepository = receiptRepository;
            this.ReceiptDetailRepository = receiptDetailRepository;
        }

        public ICustomerRepository CustomerRepository { get; }

        public IPersonRepository PersonRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IProductCategoryRepository ProductCategoryRepository { get; }

        public IReceiptRepository ReceiptRepository { get; }

        public IReceiptDetailRepository ReceiptDetailRepository { get; }

        public Task SaveAsync()
        {
            return this.dbContext.SaveChangesAsync();
        }
    }
}
