using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.IRepositories;
using Abstraction.Models;
using AutoMapper;
using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CustomerRepository : AbstractRepository<Customer, CustomerModel>, ICustomerRepository
    {
        public CustomerRepository(TradeMarketDbContext context, IMapper mapper)
            : base(context, mapper)
        {
            ArgumentNullException.ThrowIfNull(context);
        }

        public async Task<IEnumerable<CustomerModel>> GetAllWithDetailsAsync()
        {
            var result = await this.Context.Set<Customer>()
                .Include(e => e.Person)
                .Include(e => e.Receipts)
                    .ThenInclude(r => r.ReceiptDetails)
                .ToListAsync();

            return this.Mapper.Map<IEnumerable<CustomerModel>>(result);
        }

        public async Task<CustomerModel> GetByIdWithDetailsAsync(int id)
        {
            var result = await this.Context.Set<Customer>()
                .Include(e => e.Receipts)
                    .ThenInclude(r => r.ReceiptDetails)
                .Include(e => e.Person)
                .FirstOrDefaultAsync(e => e.Id == id);

            return this.Mapper.Map<CustomerModel>(result);
        }
    }
}
