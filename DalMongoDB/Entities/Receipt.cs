using System.ComponentModel.DataAnnotations.Schema;
using Abstraction.IEntities;

namespace DalMongoDB.Entities
{
    [Table("Receipt")]
    public class Receipt : BaseEntity, IReceipt
    {
        public Receipt()
            : base()
        {
            this.Customer = new Customer();
        }

        public Receipt(int id)
            : base(id)
        {
            this.Customer = new Customer();
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
