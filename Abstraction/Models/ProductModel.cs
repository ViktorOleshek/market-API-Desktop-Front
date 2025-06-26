namespace Abstraction.Models;

public class ProductModel : BaseModel
{
    public ProductModel()
        : base()
    {
    }

    public ProductModel(int id, int productCategoryId, string categoryName, string productName, decimal price)
        : base(id)
    {
        this.ProductCategoryId = productCategoryId;
        this.CategoryName = categoryName;
        this.ProductName = productName;
        this.Price = price;
    }

    public int ProductCategoryId { get; set; }

    public string CategoryName { get; set; }

    public string ProductName { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<int> ReceiptDetailIds { get; set; }
}
