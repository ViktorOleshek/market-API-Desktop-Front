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
    public class ReceiptDetailRepository : AbstractRepository<IReceiptDetail>, IReceiptDetailRepository
    {
        public ReceiptDetailRepository(TradeMarketDbContext context)
            : base(context)
        {
            ArgumentNullException.ThrowIfNull(context);
        }

        public override IReceiptDetail CreateEntity()
        {
            return new ReceiptDetail();
        }

        public async Task<IEnumerable<IReceiptDetail>> GetAllWithDetailsAsync()
        {
            return await this.Context.Set<ReceiptDetail>()
                .Include(e => e.Product)
                    .ThenInclude(e => e.Category)
                .Include(e => e.Receipt)
                .ToListAsync();
        }
    }
}
