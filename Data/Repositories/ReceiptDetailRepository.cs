using Abstraction.Entities;
using Abstraction.IRepositories;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories;

public class ReceiptDetailRepository
    : AbstractRepository<ReceiptDetail>, IReceiptDetailRepository
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
