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

        // MongoDB will use composite keys by referencing both ReceiptId and ProductId
        public int ReceiptId { get; set; }
        public int ProductId { get; set; }

        public decimal DiscountUnitPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        // These will be populated using MongoDB Aggregation when needed
        public virtual Receipt Receipt { get; set; }
        public virtual Product Product { get; set; }

        // MongoDB doesn't use [ForeignKey] attribute, but we can manually manage the relationships.
        // In aggregate queries, we would use these fields for lookup joins.
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

        // Use composite key for MongoDB - you can construct it manually as a unique identifier if needed
        public (int ReceiptId, int ProductId) CompositeKey => (ReceiptId, ProductId);
    }
}
