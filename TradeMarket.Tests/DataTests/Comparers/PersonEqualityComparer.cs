using Data.Entities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TradeMarket.Tests
{
    internal sealed class PersonEqualityComparer : IEqualityComparer<Person>
    {
        public bool Equals([AllowNull] Person x, [AllowNull] Person y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.Name == y.Name
                && x.Surname == y.Surname
                && x.BirthDate == y.BirthDate;
        }

        public int GetHashCode([DisallowNull] Person obj)
        {
            return obj.GetHashCode();
        }
    }
}
