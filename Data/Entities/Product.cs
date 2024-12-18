﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Abstraction.IEntities;

namespace Data.Entities
{
    [Table("Product")]
    public class Product : BaseEntity, IProduct
    {
        public Product()
            : base()
        {
            Category = new ProductCategory();
        }

        public Product(int id)
            : base(id)
        {
            Category = new ProductCategory();
        }

        [Column("ProductCategoryId")]
        [ForeignKey(nameof(Category))]
        public int ProductCategoryId { get; set; }

        [Column("ProductName")]
        public string ProductName { get; set; }

        [Column("Price")]
        public decimal Price { get; set; }

        public virtual ProductCategory Category { get; set; }

        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; init; }

        IProductCategory IProduct.Category
        {
            get => this.Category;
            set => this.Category = value as ProductCategory ?? throw new ArgumentException("Value must be of type ProductCategory");
        }

        ICollection<IReceiptDetail> IProduct.ReceiptDetails
        {
            get => this.ReceiptDetails.Cast<IReceiptDetail>().ToList();
            init => this.ReceiptDetails = value.Select(rd => rd as ReceiptDetail).Where(rd => rd != null).ToList();
        }
    }
}
