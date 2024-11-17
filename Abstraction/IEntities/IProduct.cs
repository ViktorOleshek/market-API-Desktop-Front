using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.IEntities
{
    public interface IProduct : IBaseEntity
    {
        public int ProductCategoryId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public IProductCategory Category { get; set; }

        public ICollection<IReceiptDetail> ReceiptDetails { get; init; }
    }
}
