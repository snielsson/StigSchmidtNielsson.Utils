using System;
using System.IO;
using Jil;
namespace StigSchmidtNielsson.Utils.Extensions {
    /// <summary>
    /// Various general purpose extensions of the System.Object class, ie. it applies to all kinds of objects.
    /// </summary>
    public static class ObjectExtensions {
        private static readonly Options _jilCloneOptions = new Options(false, false, false, DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, true);
        private static readonly Options _jilPrettyOptions = new Options(true, false, false, DateTimeFormat.NewtonsoftStyleMillisecondsSinceUnixEpoch, true);
        /// <summary>
        /// Deep clones an object by using the high performance JSON serializer JIL.
        /// </summary>
        /// <typeparam name="T">Type of the object to clone.</typeparam>
        /// <param name="obj">The object to clone.</param>
        /// <returns>A deep clone of the object argument.</returns>
        public static T Clone<T>(this T obj) {
            return JSON.Deserialize<T>(JSON.Serialize(obj, _jilCloneOptions));
        }

        /// <summary>
        /// Deep equality by comparing the JSON representation of two objects.
        /// </summary>
        /// <typeparam name="T">Type of the object to compare.</typeparam>
        /// <param name="obj">The object to compare.</param>
        /// <param name="other">The other object compare.</param>
        /// <returns>True if the objects JSON representations are equal.</returns>
        public static bool IsJsonEqualTo<T>(this T obj, T other) {
            if (ReferenceEquals(obj, other)) return true;
            if (obj.GetType() != other.GetType()) return false;
            return JSON.Serialize(obj, _jilCloneOptions) == JSON.Serialize(other, _jilCloneOptions);
        }

        /// <summary>
        /// Serializes an object to (formatted) JSON using the high performance JSON serializer JIL.
        /// </summary>
        /// <typeparam name="T">Type of the object being serialised.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The object argument as a JSON string.</returns>
        public static string ToJson<T>(this T obj) {
            return JSON.Serialize(obj, _jilCloneOptions);
        }

        /// <summary>
        /// Serializes an object to pretty printet (formatted) JSON using the high performance JSON serializer JIL.
        /// </summary>
        /// <typeparam name="T">Type of the object being serialised.</typeparam>
        /// <param name="this">The object to serialize.</param>
        /// <returns>The object argument as a pretty printet (formattet) JSON string.</returns>
        public static string ToPrettyJson<T>(this T @this) {
            return JSON.Serialize(@this, _jilPrettyOptions);
        }

        /// <summary>
        /// Writes the argument as pretty printed JSON to the given TextWriter, which defaults to Console.Out.
        /// </summary>
        /// <typeparam name="T">Type of the arguments being serialized to JSON and dumped.</typeparam>
        /// <param name="this">The argument to dump as JSON.</param>
        /// <param name="writer">The TextWriter to write the JSON to, default to Console.Out.</param>
        public static void Dump<T>(this T @this, TextWriter writer = null) {
            writer = writer ?? Console.Out;
            writer.WriteLine(@this.ToPrettyJson());
        }
    }
}