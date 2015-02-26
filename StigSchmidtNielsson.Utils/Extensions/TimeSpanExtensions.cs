using System;
namespace StigSchmidtNielsson.Utils.Extensions {
    public static class TimeSpanExtensions {
        public static TimeSpan Next(this TimeSpan @this, DateTime dateTime) {
            return new TimeSpan(@this.Ticks*(1 + dateTime.Ticks/@this.Ticks));
        }
        public static TimeSpan Prev(this TimeSpan @this, DateTime dateTime) {
            return new TimeSpan(@this.Ticks*(dateTime.Ticks/@this.Ticks - 1));
        }
    }
}