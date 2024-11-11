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
            return await this.Context.Set<Customer>()
                .Include(e => e.Person)
                .Include(e => e.Receipts)
                    .ThenInclude(r => r.ReceiptDetails)
                .ToListAsync();
        }

        public Task<CustomerModel> GetByIdWithDetailsAsync(int id)
        {
            return this.Context.Set<Customer>()
                .Include(e => e.Receipts)
                    .ThenInclude(r => r.ReceiptDetails)
                .Include(e => e.Person)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
