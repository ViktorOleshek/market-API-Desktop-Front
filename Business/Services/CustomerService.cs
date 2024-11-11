using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstraction.IRepositories;
using Abstraction.IServices;
using Abstraction.Models;
using AutoMapper;
using Business.Validation;

namespace Business.Services
{
    public class CustomerService : AbstractService<CustomerModel>, ICustomerService
    {
        public CustomerService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            return await this.UnitOfWork.CustomerRepository.GetAllWithDetailsAsync();
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            return await this.UnitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
        }

        //public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        //{
        //    var customers = await this.UnitOfWork.CustomerRepository.GetAllWithDetailsAsync();
        //    IEnumerable<CustomerModel> customersWithProduct = customers.Where(c => c.Receipts.Any(r =>
        //        r.ReceiptDetails.Any(rd => rd.ProductId == productId)));
        //    return customersWithProduct;
        //}

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            // Отримуємо всі квитанції разом з деталями та клієнтами
            var receipts = await this.UnitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            // Знаходимо клієнтів, у яких є квитанції з продуктом, що відповідає productId
            var customerIdsWithProduct = receipts
                .Where(r => r.ReceiptDetailsIds
                    .Any(rdId => r.ReceiptDetails.Any(rd => rd.ProductId == productId)))
                .Select(r => r.CustomerId)
                .Distinct();

            // Отримуємо клієнтів за знайденими ідентифікаторами
            var customers = await this.UnitOfWork.CustomerRepository.GetByIdAsync(customerIdsWithProduct);

            return customers;
        }

        protected override ICustomerRepository GetRepository()
        {
            return this.UnitOfWork.CustomerRepository;
        }

        protected override void Validation(CustomerModel model)
        {
            var projectCreationDate = new DateTime(1950, 1, 1);

            if (model == null
                || string.IsNullOrWhiteSpace(model.Name)
                || string.IsNullOrWhiteSpace(model.Surname)
                || model.BirthDate > DateTime.UtcNow
                || model.BirthDate < projectCreationDate)
            {
                throw new MarketException();
            }
        }
    }
}
