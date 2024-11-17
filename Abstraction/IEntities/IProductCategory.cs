using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.IEntities
{
    public interface IProductCategory : IBaseEntity
    {
        public string CategoryName { get; set; }
        public ICollection<IProduct> Products { get; init; }
    }
}
