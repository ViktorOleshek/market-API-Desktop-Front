using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abstraction.IEntities;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities
{
    [Table("ReceiptDetail")]
    [PrimaryKey(nameof(ReceiptId), nameof(ProductId))]
    public class ReceiptDetail : BaseEntity, IReceiptDetail
    {
        public ReceiptDetail()
            : base()
        {
            Receipt = new Receipt();
            Product = new Product();
        }

        public ReceiptDetail(int id)
            : base(id)
        {
            Receipt = new Receipt();
            Product = new Product();
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

        IReceipt IReceiptDetail.Receipt
        {
            get => this.Receipt;
            set => this.Receipt = value as Receipt ?? throw new ArgumentException("Value must be of type Receipt");
        }

        IProduct IReceiptDetail.Product
        {
            get => this.Product;
            set => this.Product = value as Product ?? throw new ArgumentException("Value must be of type Product");
        }
    }
}
