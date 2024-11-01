using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CustomerRepository : AbstractRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(TradeMarketDbContext context)
            : base(context)
        {
            ArgumentNullException.ThrowIfNull(context);
        }

        public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            return await this.Context.Set<Customer>()
                .Include(e => e.Person)
                .Include(e => e.Receipts)
                    .ThenInclude(r => r.ReceiptDetails)
                .ToListAsync();
        }

        public Task<Customer> GetByIdWithDetailsAsync(int id)
        {
            return this.Context.Set<Customer>()
                .Include(e => e.Receipts)
                    .ThenInclude(r => r.ReceiptDetails)
                .Include(e => e.Person)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
