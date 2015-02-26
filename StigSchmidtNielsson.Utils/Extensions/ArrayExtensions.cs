using System;
namespace StigSchmidtNielsson.Utils.Extensions {
    /// <summary>
    /// Various helper methods for managing basic arrays.
    /// </summary>
    public static class ArrayExtensions {
        /// <summary>
        /// Extends an array with the given count of elements, by creating a new larger array and copy the given array to the back of the new array.
        /// </summary>
        /// <typeparam name="T">Type of array.</typeparam>
        /// <param name="this">The array to extend.</param>
        /// <param name="count">The number of elements to extend the array with.</param>
        /// <returns>A new array instance with the original array copied to the back.</returns>
        public static T[] ExtendAtFront<T>(this T[] @this, int count) {
            var tmp = new T[@this.Length + count];
            Array.Copy(@this, 0, tmp, count, @this.Length);
            return tmp;
        }

        /// <summary>
        /// Extends an array with the given count of elements, by creating a new larger array and copy the given array to the front of the new array.
        /// </summary>
        /// <typeparam name="T">Type of array.</typeparam>
        /// <param name="this">The array to extend.</param>
        /// <param name="count">The number of elements to extend the array with.</param>
        /// <returns>A new array instance with the original array copied to the front.</returns>
        public static T[] ExtendAtBack<T>(this T[] @this, int count) {
            var tmp = new T[@this.Length + count];
            Array.Copy(@this, 0, tmp, 0, @this.Length);
            return tmp;
        }

        /// <summary>
        /// Gets the last element of the array.
        /// </summary>
        /// <typeparam name="T">Type of the array elements.</typeparam>
        /// <param name="this">The array to get element from.</param>
        /// <param name="throwOnEmpty">If false(default), default value of elements type is returned otherwise an InvalidOperationException is thrown.</param>
        /// <returns>The last element or the default value of the array element type.</returns>
        public static T Last<T>(this T[] @this, bool throwOnEmpty = false) {
            var length = @this.Length;
            if (length == 0 && throwOnEmpty) throw new InvalidOperationException("Array is empty");
            return length == 0 ? default(T) : @this[length - 1];
        }
    }
}