using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    [Table("Receipt")]
    public class Receipt : BaseEntity
    {
        public Receipt()
            : base()
        {
        }

        public Receipt(int id)
            : base(id)
        {
        }

        public int CustomerId { get; set; }

        public DateTime OperationDate { get; set; }

        public bool IsCheckedOut { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; init; }
    }
}
