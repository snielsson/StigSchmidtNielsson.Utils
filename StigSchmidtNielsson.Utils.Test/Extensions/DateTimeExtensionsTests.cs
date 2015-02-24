using System;
using StigSchmidtNielsson.Utils.Extensions;
using Xunit;
namespace StigSchmidtNielsson.Utils.Test.Extensions {
    public class DateTimeExtensionsTests {
        private DateTime _dateTime = new DateTime(2015, 1, 1, 1, 1, 1, 1);
        private DateTime _localDateTime = new DateTime(2015, 1, 1, 1, 1, 1, 1, DateTimeKind.Local);
        private DateTime _utcDateTime = new DateTime(2015, 1, 1, 1, 1, 1, 1, DateTimeKind.Utc);
        private DateTime _unspecifiedDateTime = new DateTime(2015, 1, 1, 1, 1, 1, 1, DateTimeKind.Unspecified);

        [Fact]
        public void RoundToTimeSpanStartWorks() {
            Assert.Equal(new DateTime(2015, 1, 1, 1, 0, 0, 0), _dateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1)));
            Assert.Equal(new DateTime(2015, 1, 1, 1, 0, 0, 0, DateTimeKind.Local), _localDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1)));
            Assert.Equal(new DateTime(2015, 1, 1, 1, 0, 0, 0, DateTimeKind.Utc), _utcDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1)));
            Assert.Equal(new DateTime(2015, 1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), _unspecifiedDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1)));

            Assert.Equal(new DateTime(2015, 1, 1, 2, 0, 0, 0), _dateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1), 1));
            Assert.Equal(new DateTime(2015, 1, 1, 2, 0, 0, 0, DateTimeKind.Local), _localDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1), 1));
            Assert.Equal(new DateTime(2015, 1, 1, 2, 0, 0, 0, DateTimeKind.Utc), _utcDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1), 1));
            Assert.Equal(new DateTime(2015, 1, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), _unspecifiedDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1), 1));

            Assert.Equal(new DateTime(2015, 1, 1, 0, 0, 0, 0), _dateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1), -1));
            Assert.Equal(new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), _localDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1), -1));
            Assert.Equal(new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), _utcDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1), -1));
            Assert.Equal(new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), _unspecifiedDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1), -1));
        }
        [Fact]
        public void RoundToTimeSpanEndWorks() {
            Assert.Equal(new DateTime(2015, 1, 1, 2, 0, 0, 0), _dateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1)));
            Assert.Equal(new DateTime(2015, 1, 1, 2, 0, 0, 0, DateTimeKind.Local), _localDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1)));
            Assert.Equal(new DateTime(2015, 1, 1, 2, 0, 0, 0, DateTimeKind.Utc), _utcDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1)));
            Assert.Equal(new DateTime(2015, 1, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), _unspecifiedDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1)));

            Assert.Equal(new DateTime(2015, 1, 1, 3, 0, 0, 0), _dateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1), 1));
            Assert.Equal(new DateTime(2015, 1, 1, 3, 0, 0, 0, DateTimeKind.Local), _localDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1), 1));
            Assert.Equal(new DateTime(2015, 1, 1, 3, 0, 0, 0, DateTimeKind.Utc), _utcDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1), 1));
            Assert.Equal(new DateTime(2015, 1, 1, 3, 0, 0, 0, DateTimeKind.Unspecified), _unspecifiedDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1), 1));

            Assert.Equal(new DateTime(2015, 1, 1, 1, 0, 0, 0), _dateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1), -1));
            Assert.Equal(new DateTime(2015, 1, 1, 1, 0, 0, 0, DateTimeKind.Local), _localDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1), -1));
            Assert.Equal(new DateTime(2015, 1, 1, 1, 0, 0, 0, DateTimeKind.Utc), _utcDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1), -1));
            Assert.Equal(new DateTime(2015, 1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), _unspecifiedDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1), -1));
        }

        [Fact]
        public void IsInTheFutureWorks() {
            var lastYear = DateTime.Now.AddYears(-1);
            var nextYear = DateTime.Now.AddYears(1);
            Assert.False(lastYear.IsInTheFuture());
            Assert.True(nextYear.IsInTheFuture());
            Assert.False(nextYear.IsInTheFuture(() => DateTime.Now.AddYears(2)));
            Assert.True(lastYear.IsInTheFuture(() => DateTime.Now.AddYears(-2)));
            Assert.False(lastYear.IsInTheFuture(() => lastYear));
            Assert.False(nextYear.IsInTheFuture(() => nextYear));
        }

        [Fact]
        public void IsInThePastWorks() {
            var lastYear = DateTime.Now.AddYears(-1);
            var nextYear = DateTime.Now.AddYears(1);
            Assert.True(lastYear.IsInThePast());
            Assert.False(nextYear.IsInThePast());
            Assert.True(nextYear.IsInThePast(() => DateTime.Now.AddYears(2)));
            Assert.False(lastYear.IsInThePast(() => DateTime.Now.AddYears(-2)));
            Assert.False(lastYear.IsInThePast(() => lastYear));
            Assert.False(nextYear.IsInThePast(() => nextYear));
        }
        [Fact]
        public void ToAndFromUnixTimeWorks() {
            var now = DateTime.UtcNow;
            var nowRoundedToSeconds = now.RoundToTimeSpanStart(TimeSpan.FromSeconds(1));
            var unixTime = now.ToUnixTime();
            Assert.Equal(nowRoundedToSeconds, DateTimeExtensions.EPOCH.AddTicks(new DateTime(unixTime*TimeSpan.TicksPerSecond).Ticks));
            Assert.Equal(nowRoundedToSeconds, unixTime.FromUnixTime());
        }
        [Fact]
        public void ToAndFromUnixTimeMsWorks() {
            var now = DateTime.UtcNow;
            var nowRoundedToMilliSeconds = now.RoundToTimeSpanStart(TimeSpan.FromMilliseconds(1));
            var unixTimeMs = now.ToUnixTimeMs();
            Assert.Equal(nowRoundedToMilliSeconds, DateTimeExtensions.EPOCH.AddTicks(new DateTime(unixTimeMs * TimeSpan.TicksPerMillisecond).Ticks));
            Assert.Equal(nowRoundedToMilliSeconds, unixTimeMs.FromUnixTimeMs());
        }
    }
}