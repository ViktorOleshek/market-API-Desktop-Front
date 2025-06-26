using Abstraction.Entities;
using Abstraction.IRepositories;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories;

public class ReceiptRepository
    : AbstractRepository<Receipt>, IReceiptRepository
{
    public ReceiptRepository(TradeMarketDbContext context)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }

    public async Task<IEnumerable<Receipt>> GetReceiptsByCustomerIdAsync(int customerId)
    {
        return await this.Context.Set<Receipt>()
            .Include(e => e.ReceiptDetails)
                .ThenInclude(e => e.Product)
                    .ThenInclude(e => e.Category)
            .Include(e => e.Customer)
                .ThenInclude(e => e.Person)
            .Where(r => r.CustomerId == customerId)
            .ToListAsync();
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

    public async Task<Receipt> GetByIdWithDetailsAsync(int id)
    {
        return await this.Context.Set<Receipt>()
            .Include(e => e.ReceiptDetails)
                .ThenInclude(e => e.Product)
                    .ThenInclude(e => e.Category)
            .Include(e => e.Customer)
                .ThenInclude(e => e.Person)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}
