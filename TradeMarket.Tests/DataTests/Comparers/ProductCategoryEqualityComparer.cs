using Data.Entities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TradeMarket.Tests
{
     internal sealed class ProductCategoryEqualityComparer : IEqualityComparer<ProductCategory>
    {
        public bool Equals([AllowNull] ProductCategory x, [AllowNull] ProductCategory y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.CategoryName == y.CategoryName;
        }

        public int GetHashCode([DisallowNull] ProductCategory obj)
        {
            return obj.GetHashCode();
        }
    }
}
