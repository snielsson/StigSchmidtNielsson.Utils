using System;
using StigSchmidtNielsson.Utils.Extensions;
using Xunit;
namespace StigSchmidtNielsson.Utils.Test.Extensions {
    public class ArrayExtensionsTests {
        [Fact]
        public void ExtendAtFrontWorks() {
            var sut = new[] {1, 2, 3};
            Assert.Equal(3, sut.Length);
            var newSut = sut.ExtendAtFront(5);
            Assert.Equal(8, newSut.Length);
            Assert.Equal(0, newSut[0]);
            Assert.Equal(0, newSut[1]);
            Assert.Equal(0, newSut[2]);
            Assert.Equal(0, newSut[3]);
            Assert.Equal(0, newSut[4]);
            Assert.Equal(1, newSut[5]);
            Assert.Equal(2, newSut[6]);
            Assert.Equal(3, newSut[7]);

            var sut2 = new[] {"1", "2", "3"};
            Assert.Equal(3, sut2.Length);
            var newSut2 = sut2.ExtendAtFront(5);
            Assert.Equal(8, newSut2.Length);
            Assert.Null(newSut2[0]);
            Assert.Null(newSut2[1]);
            Assert.Null(newSut2[2]);
            Assert.Null(newSut2[3]);
            Assert.Null(newSut2[4]);
            Assert.Equal("1", newSut2[5]);
            Assert.Equal("2", newSut2[6]);
            Assert.Equal("3", newSut2[7]);
        }

        [Fact]
        public void ExtendAtBackWorks() {
            var sut = new[] {1, 2, 3};
            Assert.Equal(3, sut.Length);
            var newSut = sut.ExtendAtBack(5);
            Assert.Equal(8, newSut.Length);
            Assert.Equal(1, newSut[0]);
            Assert.Equal(2, newSut[1]);
            Assert.Equal(3, newSut[2]);
            Assert.Equal(0, newSut[3]);
            Assert.Equal(0, newSut[4]);
            Assert.Equal(0, newSut[5]);
            Assert.Equal(0, newSut[6]);
            Assert.Equal(0, newSut[7]);

            var sut2 = new[] {"1", "2", "3"};
            Assert.Equal(3, sut2.Length);
            var newSut2 = sut2.ExtendAtBack(5);
            Assert.Equal(8, newSut2.Length);
            Assert.Equal("1", newSut2[0]);
            Assert.Equal("2", newSut2[1]);
            Assert.Equal("3", newSut2[2]);
            Assert.Null(newSut2[3]);
            Assert.Null(newSut2[4]);
            Assert.Null(newSut2[5]);
            Assert.Null(newSut2[6]);
            Assert.Null(newSut2[7]);
        }

        [Fact]
        public void LastWorks() {
            var sut = new[] {1, 2, 3};
            Assert.Equal(3, sut.Last());
            var sut2 = new int[0];
            Assert.Equal(0, sut2.Last());
            try {
                Assert.Equal(0, sut2.Last(true));
                throw new Exception("This line should not be reached.");
            }
            catch (InvalidOperationException) {}

            var sut3 = new[] {"1", "2", "3"};
            Assert.Equal("3", sut3.Last());
            var sut4 = new string[0];
            Assert.Null(sut4.Last());
            try {
                Assert.Null(sut4.Last(true));
                throw new Exception("This line should not be reached.");
            }
            catch (InvalidOperationException) {}
        }
    }
}