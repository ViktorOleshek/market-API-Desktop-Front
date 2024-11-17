using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.IEntities
{
    public interface IReceipt : IBaseEntity
    {
        public int CustomerId { get; set; }

        public DateTime OperationDate { get; set; }

        public bool IsCheckedOut { get; set; }

        public ICustomer Customer { get; set; }

        public ICollection<IReceiptDetail> ReceiptDetails { get; init; }
    }
}
