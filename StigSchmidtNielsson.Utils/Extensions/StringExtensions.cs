using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
namespace StigSchmidtNielsson.Utils.Extensions {
    public static class StringExtensions {
        /// <summary>
        /// Encode string to Base64.
        /// </summary>
        /// <param name="this">The string to encode.</param>
        /// <returns>String encoded to Base64.</returns>
        public static string Base64Encode(this string @this) {
            var plainTextBytes = Encoding.UTF8.GetBytes(@this);
            return Convert.ToBase64String(plainTextBytes);
        }
        /// <summary>
        /// Decode Base64 string.
        /// </summary>
        /// <param name="this">The Base64 string to decode.</param>
        /// <returns>The decoded string value.</returns>
        public static string Base64Decode(this string @this) {
            var base64EncodedBytes = Convert.FromBase64String(@this);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        /// <summary>
        /// Converts a byte array to a string of hex chars.
        /// </summary>
        /// <param name="bytes">The bytes to represent as a string.</param>
        /// <returns>The byte array as a string of hex chars.</returns>
        public static string ToHexString(this byte[] bytes) {
            var c = new char[bytes.Length*2];
            for (var i = 0; i < bytes.Length; ++i) {
                var b = ((byte) (bytes[i] >> 4));
                c[i*2] = (char) (b > 9 ? b + 0x37 : b + 0x30);
                b = ((byte) (bytes[i] & 0xF));
                c[i*2 + 1] = (char) (b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(c);
        }
        /// <summary>
        /// Converts a string of hex chars to a byte array. Is not robust against bad input, so only pass in strings with an event number of chars in the rnage [0123456789ABCDEF].
        /// </summary>
        /// <param name="hexString">The hexstring to convert to a byte array.</param>
        /// <returns>The byte array of the hexstring.</returns>
        public static byte[] HexStringToByteArray(this string hexString) {
            var hexStringLength = hexString.Length;
            if(hexStringLength%2 == 1) throw new ArgumentException("hexString is not valid. Must contain an even number of chars in the range [0123456789ABCDEF].");
            var b = new byte[hexStringLength/2];
            for (var i = 0; i < hexStringLength; i += 2) {
                var topChar = (hexString[i] > 0x40 ? hexString[i] - 0x37 : hexString[i] - 0x30) << 4;
                var bottomChar = hexString[i + 1] > 0x40 ? hexString[i + 1] - 0x37 : hexString[i + 1] - 0x30;
                b[i / 2] = Convert.ToByte(topChar + bottomChar);
            }
            return b;
        }

        #region Regex stuff
        private static readonly Lazy<ConcurrentDictionary<string, Regex>> _regexCache = new Lazy<ConcurrentDictionary<string, Regex>>(() => new ConcurrentDictionary<string, Regex>());
        /// <summary>
        /// The RegexOptions to se when auto creating new regex objects from the regex related string extension methods. Default value is RegexOptions.Compiled.
        /// </summary>
        public static RegexOptions DefaultRegexOptions = RegexOptions.Compiled;
        private static Regex GetCachedRegex(string pattern, RegexOptions regexOptions) {
            var key = pattern + (int) regexOptions;
            return _regexCache.Value.GetOrAdd(key, _ => new Regex(pattern, regexOptions));
        }

        /// <summary>
        /// Extracts an IEnumerable of all matches of a single match group int the pattern regex. Default match group index is 1.  Optimized to use an internal regex cache, shared by all regex related string extension methods of this library.
        /// </summary>
        /// <param name="this">The string to match against pattern</param>
        /// <param name="pattern">The pattern to construct a regex from. The Regex will be cached for future matches.</param>
        /// <param name="group">The index of the group match to extract. </param>
        /// <param name="options">Optional regex options to use when constructing the regex from the pattern.</param>
        /// <returns>An IEnumerable of all matches of the given match group.</returns>
        public static IEnumerable<string> ExtractSingleGroup(this string @this, string pattern, int group = 1, RegexOptions? options = null) {
            var matches = GetCachedRegex(pattern, options ?? DefaultRegexOptions).Matches(@this);
            if (matches.Count == 0) yield break;
            for (var i = 0; i < matches.Count; i++) {
                var match = matches[i];
                yield return match.Groups[group].Value;
            }
        }

        /// <summary>
        /// Extracts an IEnumerable arrays of matches from all match groups in the pattern regex. Optimized to use an internal regex cache, shared by all regex related string extension methods of this library.
        /// </summary>
        /// <param name="this">The string to match against pattern.</param>
        /// <param name="pattern">The pattern to construct a regex from. The Regex will be cached for future matches.</param>
        /// <param name="options">Optional regex options to use when constructing the regex from the pattern.</param>
        /// <returns>An IEnumerable of array of matches of all matches groups.</returns>
        public static IEnumerable<string[]> ExtractAllGroups(this string @this, string pattern, RegexOptions? options = null) {
            var matches = GetCachedRegex(pattern, options ?? DefaultRegexOptions).Matches(@this);
            if (matches.Count == 0) yield break;
            for (var i = 0; i < matches.Count; i++) {
                var match = matches[i];
                var values = new string[match.Groups.Count];
                for (var j = 0; j < match.Groups.Count; j++) {
                    values[j] = match.Groups[j].Value;
                }
                yield return values;
            }
        }

        /// <summary>
        /// Shorthand helper to make pattern based string replacement, optimized to use an internal regex cache, shared by all regex related string extension methods of this library.
        /// </summary>
        /// <param name="this">The string to make replacements in. This string will not change, but a modified copied will be returned.</param>
        /// <param name="pattern">The pattern to match.</param>
        /// <param name="replacement">The string to replace all matches with.</param>
        /// <param name="options">Optional regex options to use when constructing the regex from the pattern.</param>
        /// <returns>A modified copy of input string with all matches replaced with replacement string.</returns>
        public static string Replace(this string @this, string pattern, string replacement, RegexOptions? options = null) {
            return GetCachedRegex(pattern, options ?? DefaultRegexOptions).Replace(@this, replacement);
        }

        /// <summary>
        /// Shorthand helper to make pattern based string splitting, optimized to use an internal regex cache, shared by all regex related string extension methods of this library.
        /// </summary>
        /// <param name="this">The string to split.</param>
        /// <param name="pattern">The pattern to split on.</param>
        /// <param name="options">Optional regex options to use when constructing the regex from the pattern.</param>
        /// <param name="count">Optional count of elements to include in result.</param>
        /// <param name="startat"></param>
        /// <returns>Array of strings with the results of splitting the input string on the pattern.</returns>
        public static string[] Split(this string @this, string pattern, RegexOptions? options = null, int count = 0, int startat = 0) {
            return GetCachedRegex(pattern, options ?? DefaultRegexOptions).Split(@this, count, startat);
        }

        /// <summary>
        /// Tests if string matches a pattern, optimized to use an internal regex cache, shared by all regex related string extension methods of this library.
        /// </summary>
        /// <param name="this">The string to test.</param>
        /// <param name="pattern">The pattern to use in test.</param>
        /// <param name="options">Optional regex options to use when constructing the regex from the pattern.</param>
        /// <returns>True if string matches, false otherwise.</returns>
        public static bool Matches(this string @this, string pattern, RegexOptions? options = null) {
            var regex = GetCachedRegex(pattern, options ?? DefaultRegexOptions);
            return regex.IsMatch(@this);
        }

        /// <summary>
        /// Get the match result of mathcing string against pattern, optimized to use an internal regex cache, shared by all regex related string extension methods of this library.
        /// </summary>
        /// <param name="this">The string to match.</param>
        /// <param name="pattern">The pattern to match against.</param>
        /// <param name="options">Optional regex options to use when constructing the regex from the pattern.</param>
        /// <returns>The Match object result from the RegexMatch.</returns>
        public static Match Match(this string str, string pattern, RegexOptions? options = null)
        {
            var regex = GetCachedRegex(pattern, options ?? DefaultRegexOptions);
            return regex.Match(str);
        }

        /// <summary>
        /// Get the match result of mathcing string against pattern, optimized to use an internal regex cache, shared by all regex related string extension methods of this library.
        /// </summary>
        /// <param name="this">The string to match.</param>
        /// <param name="pattern">The pattern to match against.</param>
        /// <param name="match">*Out parameter to hold the match result.</param>
        /// <param name="options">Optional regex options to use when constructing the regex from the pattern.</param>
        /// <returns>True if match succeeded, false otherwise. The match out parameter contains the resulting Match object or null.</returns>
        public static bool TryMatch(this string @this, string pattern, out Match match, RegexOptions? options = null) {
            var regex = GetCachedRegex(pattern, options ?? DefaultRegexOptions);
            match = regex.Match(@this);
            return match.Success;
        }

        /// <summary>
        /// Convenience method to replace in string based on regex. Does not use the regex cache, because the regex is provided as parameter.
        /// </summary>
        /// <param name="this">The string to replace in.</param>
        /// <param name="regex">The regex used to make the replacements.</param>
        /// <param name="replacement">The string to replace with.</param>
        /// <returns>Copy of input string with replacements made.</returns>
        public static string Replace(this string @this, Regex regex, string replacement) {
            return regex.Replace(@this, replacement);
        }

        /// <summary>
        /// Convenience method to split string on regex. Does not use the regex cache, because the regex is provided as parameter.
        /// </summary>
        /// <param name="this">The string to split.</param>
        /// <param name="regex">The regex used to perfortm the split.</param>
        /// <param name="count">Optional count argument for the Regex.Split method.</param>
        /// <param name="startAt">Optional startAt argument for the Regex.Splt method.</param>
        /// <returns>Array of strings with the results of splitting the input string on the regex.</returns>
        public static string[] Split(this string @this, Regex regex, int count = 0, int startAt = 0) {
            return regex.Split(@this, count, startAt);
        }
        #endregion

        #region Misc
        public static string Diff(this string @this, string str) {
            var sb = new StringBuilder();
            var lines1 = @this.Split(Environment.NewLine);
            var lines2 = str.Split(Environment.NewLine);
            for (var i = 0; i < Math.Min(lines1.Length, lines2.Length); i++) {
                var l1 = lines1[i];
                var l2 = lines2[i];
                if (l1 != l2) {
                    var limit = Math.Min(l1.Length, l2.Length);
                    for (var j = 0; j < limit; j++) {
                        if (l1[j] != l2[j] || j == limit - 1) {
                            if (i > 1) sb.AppendFormat("{0:D4}: ", i - 1).AppendLine(lines1[i - 2]);
                            if (i > 0) sb.AppendFormat("{0:D4}: ", i).AppendLine(lines1[i - 1]);
                            sb.AppendFormat("{0:D4}: ", i + 1).AppendLine(lines1[i]);
                            sb.Append(' ', 6 + j).AppendLine("^");
                            if (lines1.Length > i + 1) sb.AppendFormat("{0:D4}: ", i + 2).AppendLine(lines1[i + 1]);
                            if (lines1.Length > i + 2) sb.AppendFormat("{0:D4}: ", i + 3).AppendLine(lines1[i + 2]);

                            sb.AppendLine(new string('-', 6 + l1.Length));

                            if (i > 1) sb.AppendFormat("{0:D4}: ", i - 1).AppendLine(lines2[i - 2]);
                            if (i > 0) sb.AppendFormat("{0:D4}: ", i).AppendLine(lines2[i - 1]);
                            sb.AppendFormat("{0:D4}: ", i + 1).AppendLine(lines2[i]);
                            sb.Append(' ', 6 + j).AppendLine("^");
                            if (lines2.Length > i + 1) sb.AppendFormat("{0:D4}: ", i + 2).AppendLine(lines2[i + 1]);
                            if (lines2.Length > i + 2) sb.AppendFormat("{0:D4}: ", i + 3).AppendLine(lines2[i + 2]);
                            break;
                        }
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates a slug.
        /// References:
        /// http://stackoverflow.com/questions/25259/how-does-stack-overflow-generate-its-seo-friendly-urls
        /// http://www.unicode.org/reports/tr15/tr15-34.html
        /// http://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
        /// http://stackoverflow.com/questions/25259/how-do-you-include-a-webpage-title-as-part-of-a-webpage-url/25486#25486
        /// http://stackoverflow.com/questions/3769457/how-can-i-remove-accents-on-a-string
        /// </summary>
        public static string ToSlug(this string @this, bool toLower = false) {
            if (@this == null)
                return "";
            var normalised = @this.Normalize(NormalizationForm.FormKD);
            const int maxlen = 80;
            var len = normalised.Length;
            var prevDash = false;
            var sb = new StringBuilder(len);
            char c;
            for (var i = 0; i < len; i++) {
                c = normalised[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9')) {
                    if (prevDash) {
                        sb.Append('-');
                        prevDash = false;
                    }
                    sb.Append(c);
                }
                else if (c >= 'A' && c <= 'Z') {
                    if (prevDash) {
                        sb.Append('-');
                        prevDash = false;
                    }
                    // Tricky way to convert to lowercase
                    if (toLower)
                        sb.Append((char) (c | 32));
                    else
                        sb.Append(c);
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' || c == '\\' || c == '-' || c == '_' || c == '=') {
                    if (!prevDash && sb.Length > 0) prevDash = true;
                }
                else {
                    var swap = ConvertEdgeCases(c, toLower);
                    if (swap != null) {
                        if (prevDash) {
                            sb.Append('-');
                            prevDash = false;
                        }
                        sb.Append(swap);
                    }
                }
                if (sb.Length == maxlen)
                    break;
            }
            return sb.ToString();
        }

        private static string ConvertEdgeCases(char c, bool toLower) {
            string swap = null;
            switch (c) {
                case 'ı':
                    swap = "i";
                    break;
                case 'ł':
                    swap = "l";
                    break;
                case 'Ł':
                    swap = toLower ? "l" : "L";
                    break;
                case 'đ':
                    swap = "d";
                    break;
                case 'ß':
                    swap = "ss";
                    break;
                case 'Ø':
                    swap = toLower ? "oe" : "Oe";
                    break;
                case 'ø':
                    swap = "oe";
                    break;
                case 'Æ':
                    swap = toLower ? "ae" : "Ae";
                    break;
                case 'æ':
                    swap = "ae";
                    break;
                case 'Å':
                    swap = toLower ? "aa" : "Aa";
                    break;
                case 'å':
                    swap = "aa";
                    break;
                case 'Þ':
                    swap = "th";
                    break;
            }
            return swap;
        }

        public static string RemoveWhitespace(this string input) {
            int j = 0, inputlen = input.Length;
            var newarr = new char[inputlen];
            for (var i = 0; i < inputlen; ++i) {
                var tmp = input[i];
                if (char.IsWhiteSpace(tmp)) continue;
                newarr[j] = tmp;
                ++j;
            }
            return new String(newarr, 0, j);
        }

        private static readonly char[] _defaultTrimeChars = {' '};
        public static string TrimOrDefault(this string @this, string nullString, params char[] trimChars) {
            trimChars = trimChars ?? _defaultTrimeChars;
            if (string.IsNullOrEmpty(@this)) return nullString;
            return @this.Trim(trimChars);
        }

        private static readonly MD5 _md5 = MD5.Create();
        public static string ToMD5Hash(this string @this) {
            lock (_md5) {
                var bytes = _md5.ComputeHash(Encoding.UTF8.GetBytes(@this));
                return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
        }
        #endregion
    }
}