using System.ComponentModel.DataAnnotations.Schema;

namespace Abstraction.Entities;

[Table("Product")]
public class Product
    : BaseEntity
{
    public Product()
        : base()
    {
    }

    public Product(int id)
        : base(id)
    {
    }

    [Column("ProductCategoryId")]
    [ForeignKey(nameof(Category))]
    public int ProductCategoryId { get; set; }

    [Column("ProductName")]
    public string ProductName { get; set; }

    [Column("Price")]
    public decimal Price { get; set; }

    public virtual ProductCategory Category { get; set; }

    public virtual ICollection<ReceiptDetail> ReceiptDetails { get; init; }
}
