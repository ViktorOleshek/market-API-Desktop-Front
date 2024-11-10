using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.IRepositories;
using Abstraction.Models;
using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ReceiptDetailRepository : AbstractRepository<ReceiptDetail, ReceiptDetailModel>, IReceiptDetailRepository
    {
        public ReceiptDetailRepository(TradeMarketDbContext context)
            : base(context)
        {
            ArgumentNullException.ThrowIfNull(context);
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetAllWithDetailsAsync()
        {
            return await this.Context.Set<ReceiptDetail>()
                .Include(e => e.Product)
                    .ThenInclude(e => e.Category)
                .Include(e => e.Receipt)
                .ToListAsync();
        }
    }
}
