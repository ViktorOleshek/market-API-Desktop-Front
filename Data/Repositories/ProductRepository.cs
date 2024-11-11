using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.IRepositories;
using Abstraction.Models;
using AutoMapper;
using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ProductRepository : AbstractRepository<Product, ProductModel>, IProductRepository
    {
        public ProductRepository(TradeMarketDbContext context, IMapper mapper)
            : base(context, mapper)
        {
            ArgumentNullException.ThrowIfNull(context);
        }

        public async Task<IEnumerable<ProductModel>> GetAllWithDetailsAsync()
        {
            var result = await this.Context.Set<Product>()
                .Include(e => e.ReceiptDetails)
                .Include(e => e.Category)
                .ToListAsync();

            return this.Mapper.Map<IEnumerable<ProductModel>>(result);
        }

        public async Task<ProductModel> GetByIdWithDetailsAsync(int id)
        {
            var result = await this.Context.Set<Product>()
                .Include(e => e.ReceiptDetails)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);

            return this.Mapper.Map<ProductModel>(result);
        }
    }
}
