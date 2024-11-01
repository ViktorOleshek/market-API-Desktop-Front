using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    [Table("ProductCategory")]
    public class ProductCategory : BaseEntity
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
    }
}
