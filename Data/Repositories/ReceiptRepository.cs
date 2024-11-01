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
    public class ReceiptRepository : AbstractRepository<Receipt>, IReceiptRepository
    {
        public ReceiptRepository(TradeMarketDbContext context)
            : base(context)
        {
            ArgumentNullException.ThrowIfNull(context);
        }

        public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            return await this.Context.Set<Receipt>()
                .Include(e => e.ReceiptDetails)
                    .ThenInclude(e => e.Product)
                        .ThenInclude(e => e.Category)
                .Include(e => e.Customer)
                    .ThenInclude(e => e.Person)
                .ToListAsync();
        }

        public Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            return this.Context.Set<Receipt>()
                .Include(e => e.ReceiptDetails)
                    .ThenInclude(e => e.Product)
                        .ThenInclude(e => e.Category)
                .Include(e => e.Customer)
                    .ThenInclude(e => e.Person)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
