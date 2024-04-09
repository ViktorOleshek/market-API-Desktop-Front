using Data.Entities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TradeMarket.Tests
{
    internal sealed class CustomerEqualityComparer : IEqualityComparer<Customer>
    {
        public bool Equals([AllowNull] Customer x, [AllowNull] Customer y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.PersonId == y.PersonId
                && x.DiscountValue == y.DiscountValue;
        }

        public int GetHashCode([DisallowNull] Customer obj)
        {
            return obj.GetHashCode();
        }
    }
}
