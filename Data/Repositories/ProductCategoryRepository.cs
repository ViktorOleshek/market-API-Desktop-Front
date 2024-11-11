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
    public class ProductCategoryRepository : AbstractRepository<ProductCategory, ProductCategoryModel>, IProductCategoryRepository
    {
        public ProductCategoryRepository(TradeMarketDbContext context, IMapper mapper)
            : base(context, mapper)
        {
            ArgumentNullException.ThrowIfNull(context);
        }
    }
}
