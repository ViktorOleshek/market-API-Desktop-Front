using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.IEntities;
using Abstraction.IRepositories;
using DalMongoDB.Data;
using DalMongoDB.Entities;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore;

namespace DalMongoDB.Repositories
{
    public class ProductCategoryRepository : AbstractRepository<IProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(IMongoDatabase database)
            : base(database, "ProductCategories")
        {
            ArgumentNullException.ThrowIfNull(database);
        }
    }
}
