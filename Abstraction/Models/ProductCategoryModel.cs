namespace Abstraction.Models;

public class ProductCategoryModel : BaseModel
{
    public ProductCategoryModel()
        : base()
    {
    }

    public ProductCategoryModel(int id, string categoryName)
        : base(id)
    {
        this.CategoryName = categoryName;
    }

    public string CategoryName { get; set; }

    public virtual ICollection<int> ProductIds { get; set; }
}
