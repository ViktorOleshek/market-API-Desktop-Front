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
    public class ProductRepository : AbstractRepository<Product, ProductModel>, IProductRepository
    {
        public ProductRepository(TradeMarketDbContext context)
            : base(context)
        {
            ArgumentNullException.ThrowIfNull(context);
        }

        public async Task<IEnumerable<ProductModel>> GetAllWithDetailsAsync()
        {
            return await this.Context.Set<Product>()
                .Include(e => e.ReceiptDetails)
                .Include(e => e.Category)
                .ToListAsync();
        }

        public Task<ProductModel> GetByIdWithDetailsAsync(int id)
        {
            return this.Context.Set<Product>()
                .Include(e => e.ReceiptDetails)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
