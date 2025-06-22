using Abstraction.Entities;
using Abstraction.IRepositories;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories;

public class ProductRepository
    : AbstractRepository<Product>, IProductRepository
{
    public ProductRepository(TradeMarketDbContext context)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }

    public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
    {
        return await this.Context.Set<Product>()
            .Include(e => e.ReceiptDetails)
            .Include(e => e.Category)
            .ToListAsync();
    }

    public async Task<Product> GetByIdWithDetailsAsync(int id)
    {
        return await this.Context.Set<Product>()
            .Include(e => e.ReceiptDetails)
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}
