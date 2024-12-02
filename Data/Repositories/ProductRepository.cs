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
    public class ProductRepository : AbstractRepository<IProduct>, IProductRepository
    {
        public ProductRepository(TradeMarketDbContext context)
            : base(context)
        {
            ArgumentNullException.ThrowIfNull(context);
        }

        public override IProduct CreateEntity()
        {
            return new Product();
        }

        public async Task<IEnumerable<IProduct>> GetAllWithDetailsAsync()
        {
            return await this.Context.Set<Product>()
                .Include(e => e.ReceiptDetails)
                .Include(e => e.Category)
                .ToListAsync();
        }

        public async Task<IProduct> GetByIdWithDetailsAsync(int id)
        {
            return await this.Context.Set<Product>()
                .Include(e => e.ReceiptDetails)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
