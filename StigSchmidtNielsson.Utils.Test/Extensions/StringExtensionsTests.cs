using System;
using StigSchmidtNielsson.Utils.Extensions;
using Xunit;
namespace StigSchmidtNielsson.Utils.Test.Extensions {
    public class StringExtensionsTests : TestBase {
        [Fact]
        public void Base64EncodeDecodeWorks() {
            var sut = Create<string>();
            var b64 = sut.Base64Encode();
            var result = b64.Base64Decode();
            Assert.Equal(sut, result);
        }

        [Fact]
        public void ToHexStringAndToByteArrayWorks() {
            var sut = new Byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20};
            var hexString = sut.ToHexString();
            var byteArray = hexString.HexStringToByteArray();
            Assert.True(sut.IsJsonEqualTo(byteArray));
        }

        #region regex stuff

        #endregion

    }
}