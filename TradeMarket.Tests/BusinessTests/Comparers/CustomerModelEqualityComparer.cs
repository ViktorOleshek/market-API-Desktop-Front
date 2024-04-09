using Data.Entities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Business.Models;
using System;

namespace TradeMarket.Tests
{
    internal sealed class CustomerModelEqualityComparer : IEqualityComparer<CustomerModel>
    {
        public bool Equals([AllowNull] CustomerModel x, [AllowNull] CustomerModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id &&
                x.BirthDate == y.BirthDate &&
                x.DiscountValue == y.DiscountValue &&
                string.Equals(x.Name, y.Name, StringComparison.Ordinal) &&
                string.Equals(x.Surname, y.Surname, StringComparison.Ordinal);
        }

        public int GetHashCode([DisallowNull] CustomerModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
