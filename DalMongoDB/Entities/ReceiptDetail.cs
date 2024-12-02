using Abstraction.IEntities;

namespace DalMongoDB.Entities
{
    public class ReceiptDetail : BaseEntity, IReceiptDetail
    {
        public ReceiptDetail()
            : base()
        {
            this.Receipt = new Receipt();
            this.Product = new Product();
        }

        public ReceiptDetail(int id)
            : base(id)
        {
            this.Receipt = new Receipt();
            this.Product = new Product();
        }

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
