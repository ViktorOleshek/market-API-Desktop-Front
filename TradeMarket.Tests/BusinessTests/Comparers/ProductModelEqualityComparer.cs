using Data.Entities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Business.Models;
using System;

namespace TradeMarket.Tests
{
    internal sealed class ProductModelEqualityComparer : IEqualityComparer<ProductModel>
    {
        public bool Equals([AllowNull] ProductModel x, [AllowNull] ProductModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id &&
                x.ProductCategoryId == y.ProductCategoryId &&
                string.Equals(x.CategoryName, y.CategoryName, StringComparison.Ordinal) &&
                string.Equals(x.ProductName, y.ProductName, StringComparison.Ordinal) &&
                x.Price == y.Price;
        }

        public int GetHashCode([DisallowNull] ProductModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
