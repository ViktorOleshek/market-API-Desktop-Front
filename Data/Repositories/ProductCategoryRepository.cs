using System;
using Abstraction.IEntities;
using Abstraction.IRepositories;
using Data.Data;
using Data.Entities;

namespace Data.Repositories
{
    public class ProductCategoryRepository : AbstractRepository<IProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(TradeMarketDbContext context)
            : base(context)
        {
            ArgumentNullException.ThrowIfNull(context);
        }

        public override IProductCategory CreateEntity()
        {
            return new ProductCategory();
        }
    }
}
