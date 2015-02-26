using System;
using System.Collections.Generic;
namespace StigSchmidtNielsson.Utils {
    /// <summary>
    /// Comparer for comparing two keys, handling equality as beeing greater
    /// Use this Comparer e.g. with SortedLists or SortedDictionaries, that don't allow duplicate keys
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable {
        #region IComparer<TKey> Members
        public int Compare(TKey x, TKey y) {
            var result = x.CompareTo(y);
            if (result == 0) return 1; // Handle equality as beeing greater
            return result;
        }
        #endregion
    }
}