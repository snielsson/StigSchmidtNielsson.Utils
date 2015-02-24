namespace StigSchmidtNielsson.Utils.Extensions {
    public static class StringExtensions {
        /// <summary>
        /// Encode given string to Base64.
        /// </summary>
        /// <param name="this">The string to convert.</param>
        /// <returns>The given string encoded as a Base64 string.</returns>
        public static string EncodeToBase64(string @this) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(@this);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        /// <summary>
        /// Decode a Base64 string.
        /// </summary>
        /// <param name="this">The Base64 string to decode.</param>
        /// <returns>The decoded string value.</returns>
        public static string DecodeFromBase64(string @this) {
            var base64EncodedBytes = System.Convert.FromBase64String(@this);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }        
    }
}