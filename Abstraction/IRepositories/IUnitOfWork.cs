namespace Abstraction.IRepositories
{
    public interface IUnitOfWork
    {
        ICustomerRepository CustomerRepository { get; }

        IPersonRepository PersonRepository { get; }

        IProductRepository ProductRepository { get; }

        IProductCategoryRepository ProductCategoryRepository { get; }

        IReceiptRepository ReceiptRepository { get; }

        IReceiptDetailRepository ReceiptDetailRepository { get; }

        IUserRepository UserRepository { get; }

        Task SaveAsync();
    }
}
