using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.IEntities;

namespace DalMongoDB.Entities
{
    [Table("ProductCategory")]
    public class ProductCategory : BaseEntity, IProductCategory
    {
        public ProductCategory()
            : base()
        {
        }

        public ProductCategory(int id)
            : base(id)
        {
        }

        [Column("CaregoryName")]
        public virtual string CategoryName { get; set; }

        public virtual ICollection<Product> Products { get; init; }

        ICollection<IProduct> IProductCategory.Products
        {
            get => this.Products.Cast<IProduct>().ToList();
            init => this.Products = value.Select(p => p as Product).Where(p => p != null).ToList();
        }
    }
}
