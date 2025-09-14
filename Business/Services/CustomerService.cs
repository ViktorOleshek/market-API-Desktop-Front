using Abstraction.Entities;
using Abstraction.IRepositories;
using Abstraction.IServices;
using Abstraction.Models;
using AutoMapper;
using Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services;

public class CustomerService
    : AbstractService<CustomerModel, Customer>, ICustomerService
{
    public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper, unitOfWork.CustomerRepository)
    {
    }

    public virtual async Task<IEnumerable<CustomerModel>> GetAllAsync()
    {
        var entities = await this.UnitOfWork.CustomerRepository.GetAllWithDetailsAsync();
        return this.Mapper.Map<IEnumerable<CustomerModel>>(entities);
    }

    public virtual async Task<CustomerModel> GetByIdAsync(int id)
    {
        var entity = await this.UnitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
        return this.Mapper.Map<CustomerModel>(entity);
    }

    public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
    {
        var customers = await this.UnitOfWork.CustomerRepository.GetAllWithDetailsAsync();
        var customersWithProduct = customers.Where(c => c.Receipts.Any(r =>
            r.ReceiptDetails.Any(rd => rd.ProductId == productId)));

        return this.Mapper.Map<IEnumerable<CustomerModel>>(customersWithProduct);
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
