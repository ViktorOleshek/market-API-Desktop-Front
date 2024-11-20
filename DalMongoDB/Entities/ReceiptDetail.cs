using System;
using System.Collections.Generic;
using Abstraction.IEntities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DalMongoDB.Entities
{
    public class ReceiptDetail : BaseEntity, IReceiptDetail
    {
        public ReceiptDetail() : base() { }

        public ReceiptDetail(int id) : base(id) { }

        public int ReceiptId { get; set; }
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

        public (int ReceiptId, int ProductId) CompositeKey => (ReceiptId, ProductId);
    }
}
