using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.IEntities;

namespace Data.Entities
{
    [Table("Receipt")]
    public class Receipt : BaseEntity, IReceipt
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

        ICustomer IReceipt.Customer
        {
            get => this.Customer;
            set => this.Customer = value as Customer ?? throw new ArgumentException("Value must be of type Customer");
        }

        ICollection<IReceiptDetail> IReceipt.ReceiptDetails
        {
            get => this.ReceiptDetails.Cast<IReceiptDetail>().ToList();
            init => this.ReceiptDetails = value.Select(rd => rd as ReceiptDetail).Where(rd => rd != null).ToList();
        }
    }
}
