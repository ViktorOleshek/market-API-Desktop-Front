using Abstraction.IRepositories;
using Data.Repositories;
using System.Threading.Tasks;

namespace Data.Data;

public class UnitOfWork
    : IUnitOfWork
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
        this.UserRepository = new UserRepository(dbContext);
    }

    public UnitOfWork(
        TradeMarketDbContext dbContext,
        ICustomerRepository customerRepository,
        IPersonRepository personRepository,
        IProductRepository productRepository,
        IProductCategoryRepository productCategoryRepository,
        IReceiptRepository receiptRepository,
        IReceiptDetailRepository receiptDetailRepository,
        IUserRepository userRepository)
    {
        this.dbContext = dbContext;
        this.CustomerRepository = customerRepository;
        this.PersonRepository = personRepository;
        this.ProductRepository = productRepository;
        this.ProductCategoryRepository = productCategoryRepository;
        this.ReceiptRepository = receiptRepository;
        this.ReceiptDetailRepository = receiptDetailRepository;
        this.UserRepository = userRepository;
    }

    public ICustomerRepository CustomerRepository { get; }

    public IPersonRepository PersonRepository { get; }

    public IProductRepository ProductRepository { get; }

    public IProductCategoryRepository ProductCategoryRepository { get; }

    public IReceiptRepository ReceiptRepository { get; }

    public IReceiptDetailRepository ReceiptDetailRepository { get; }

    public IUserRepository UserRepository { get; }

    public Task SaveAsync()
    {
        return this.dbContext.SaveChangesAsync();
    }
}
