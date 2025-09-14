using Abstraction.Entities;
using Abstraction.IRepositories;
using Data.Data;
using System;

namespace Data.Repositories;

public class ProductCategoryRepository
    : AbstractRepository<ProductCategory>, IProductCategoryRepository
{
    public ProductCategoryRepository(TradeMarketDbContext context)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }
}
