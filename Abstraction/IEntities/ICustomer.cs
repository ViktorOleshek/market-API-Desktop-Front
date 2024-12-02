using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.IEntities
{
    public interface ICustomer : IBaseEntity
    {
        public int PersonId { get; set; }

        public int DiscountValue { get; set; }
        public byte []? Photo { get; set; }
        public IPerson Person { get; set; }
        public ICollection<IReceipt> Receipts { get; init; }
    }
}
