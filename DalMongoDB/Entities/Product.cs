using System.ComponentModel.DataAnnotations.Schema;
using Abstraction.IEntities;

namespace DalMongoDB.Entities
{
    [Table("Product")]
    public class Product : BaseEntity, IProduct
    {
        public Product()
            : base()
        {
            this.Category = new ProductCategory();
        }

        public Product(int id)
            : base(id)
        {
            this.Category = new ProductCategory();
        }

        [Column("ProductCategoryId")]
        [ForeignKey(nameof(Category))]
        public int ProductCategoryId { get; set; }

        [Column("ProductName")]
        public string ProductName { get; set; }

        [Column("Price")]
        public decimal Price { get; set; }

        public virtual ProductCategory Category { get; set; }

        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }

        IProductCategory IProduct.Category
        {
            get => this.Category;
            set => this.Category = value as ProductCategory ?? throw new ArgumentException("Value must be of type ProductCategory");
        }

        ICollection<IReceiptDetail> IProduct.ReceiptDetails
        {
            get => this.ReceiptDetails.Cast<IReceiptDetail>().ToList();
            init => this.ReceiptDetails = value.Select(rd => rd as ReceiptDetail).Where(rd => rd != null).ToList();
        }
    }
}
