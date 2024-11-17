using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.IEntities
{
    public interface IReceiptDetail : IBaseEntity
    {
        public int ReceiptId { get; set; }

        public int ProductId { get; set; }

        public decimal DiscountUnitPrice { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public IReceipt Receipt { get; set; }

        public IProduct Product { get; set; }
    }
}
