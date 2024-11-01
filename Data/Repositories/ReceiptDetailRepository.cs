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
    public class ReceiptDetailRepository : AbstractRepository<ReceiptDetail>, IReceiptDetailRepository
    {
        public ReceiptDetailRepository(TradeMarketDbContext context)
            : base(context)
        {
            ArgumentNullException.ThrowIfNull(context);
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            return await this.Context.Set<ReceiptDetail>()
                .Include(e => e.Product)
                    .ThenInclude(e => e.Category)
                .Include(e => e.Receipt)
                .ToListAsync();
        }
    }
}
