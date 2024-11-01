using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities
{
    [Table("ReceiptDetail")]
    [PrimaryKey(nameof(ReceiptId), nameof(ProductId))]
    public class ReceiptDetail : BaseEntity
    {
        public ReceiptDetail()
            : base()
        {
        }

        public ReceiptDetail(int id)
            : base(id)
        {
        }

        [ForeignKey(nameof(Receipt))]
        public int ReceiptId { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public decimal DiscountUnitPrice { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public virtual Receipt Receipt { get; set; }

        public virtual Product Product { get; set; }
    }
}
