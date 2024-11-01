using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
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
}
