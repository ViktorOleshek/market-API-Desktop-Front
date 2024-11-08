using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Validation;

namespace Business.Services
{
    public class StatisticService : AbstractService<ReceiptModel, Receipt>, IStatisticService
    {
        public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {
            var receipts = await this.UnitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var customerReceipts = receipts.Where(r => r.CustomerId == customerId);

            var popularProducts = customerReceipts
                .SelectMany(cr => cr.ReceiptDetails)
                .GroupBy(rd => rd.Product)
                .OrderByDescending(g => g.Sum(rd => rd.Quantity))
                .Take(productCount)
                .Select(g => g.Key);

            return this.Mapper.Map<IEnumerable<ProductModel>>(popularProducts);
        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var receipts = await this.UnitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            var income = receipts
                .Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate)
                .SelectMany(r => r.ReceiptDetails)
                .Where(rd => rd.Product.ProductCategoryId == categoryId)
                .Sum(rd => rd.DiscountUnitPrice * rd.Quantity);

            return income;
        }

        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            var receiptDetails = await this.UnitOfWork.ReceiptDetailRepository.GetAllWithDetailsAsync();

            var mostPopularProducts = receiptDetails
                .GroupBy(rd => rd.Product)
                .OrderByDescending(rd => rd.Sum(rd => rd.Quantity))
                .Take(productCount)
                .Select(g => g.Key);

            return this.Mapper.Map<IEnumerable<ProductModel>>(mostPopularProducts);
        }

        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
        {
            var receipts = await this.UnitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            var mostValuableCustomers = receipts
                .Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate)
                .GroupBy(r => r.CustomerId)
                .Select(g => new CustomerActivityModel
                {
                    CustomerId = g.Key,
                    CustomerName = $"{g.First().Customer.Person.Name} {g.First().Customer.Person.Surname}",
                    ReceiptSum = g.Sum(r => r.ReceiptDetails.Sum(rd => rd.Quantity * rd.DiscountUnitPrice)),
                })
                .OrderByDescending(c => c.ReceiptSum)
                .Take(customerCount);

            return mostValuableCustomers;
        }

        protected override IReceiptRepository GetRepository()
        {
            return this.UnitOfWork.ReceiptRepository;
        }

        protected override void Validation(ReceiptModel model)
        {
            if (model == null)
            {
                throw new MarketException();
            }
        }
    }
}
