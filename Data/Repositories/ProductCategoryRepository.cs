using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.IEntities;
using Abstraction.IRepositories;
using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ProductCategoryRepository : AbstractRepository<IProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(TradeMarketDbContext context)
            : base(context)
        {
            ArgumentNullException.ThrowIfNull(context);
        }
    }
}
