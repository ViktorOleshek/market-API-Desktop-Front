using System.ComponentModel.DataAnnotations.Schema;

namespace Abstraction.Entities;

[Table("ProductCategory")]
public class ProductCategory
    : BaseEntity
{
    public ProductCategory()
        : base()
    {
    }

    public ProductCategory(int id)
        : base(id)
    {
    }

    [Column("CategoryName")]
    public virtual string CategoryName { get; set; }

    public virtual ICollection<Product> Products { get; init; }
}
