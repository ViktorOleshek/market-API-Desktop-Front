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
    public class ReceiptRepository : AbstractRepository<IReceipt>, IReceiptRepository
    {
        public ReceiptRepository(TradeMarketDbContext context)
            : base(context)
        {
            ArgumentNullException.ThrowIfNull(context);
        }

        public override IReceipt CreateEntity()
        {
            return new Receipt();
        }

        public async Task<IEnumerable<IReceipt>> GetAllWithDetailsAsync()
        {
            return await this.Context.Set<Receipt>()
                .Include(e => e.ReceiptDetails)
                    .ThenInclude(e => e.Product)
                        .ThenInclude(e => e.Category)
                .Include(e => e.Customer)
                    .ThenInclude(e => e.Person)
                .ToListAsync();
        }

        public async Task<IReceipt> GetByIdWithDetailsAsync(int id)
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
}
