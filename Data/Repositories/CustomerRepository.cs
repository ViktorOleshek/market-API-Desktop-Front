using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstraction.IEntities;
using Abstraction.IRepositories;
using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CustomerRepository : AbstractRepository<ICustomer>, ICustomerRepository
    {
        public CustomerRepository(TradeMarketDbContext context)
            : base(context)
        {
            ArgumentNullException.ThrowIfNull(context);
        }

        public override ICustomer CreateEntity()
        {
            return new Customer();
        }

        public async Task<IEnumerable<ICustomer>> GetAllWithDetailsAsync()
        {
            return await this.Context.Set<Customer>()
                .Include(e => e.Person)
                .Include(e => e.Receipts)
                    .ThenInclude(r => r.ReceiptDetails)
                .ToListAsync();
        }

        public async Task<ICustomer> GetByIdWithDetailsAsync(int id)
        {
            return await this.Context.Set<Customer>()
                .Include(e => e.Receipts)
                    .ThenInclude(r => r.ReceiptDetails)
                .Include(e => e.Person)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
