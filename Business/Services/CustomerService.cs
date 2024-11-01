using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class CustomerService : AbstractService<CustomerModel, Customer>, ICustomerService
    {
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var customers = await this.UnitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            var customersWithProduct = customers.Where(c => c.Receipts.Any(r =>
                r.ReceiptDetails.Any(rd => rd.ProductId == productId)));

            return this.Mapper.Map<IEnumerable<CustomerModel>>(customersWithProduct);
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
