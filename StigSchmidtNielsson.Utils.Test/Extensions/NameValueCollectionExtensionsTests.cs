using System;
using System.Collections.Specialized;
using StigSchmidtNielsson.Utils.Extensions;
using Xunit;
namespace StigSchmidtNielsson.Utils.Test.Extensions {
    public class NameValueCollectionExtensionsTests {

        public enum SomeEnum {
            Enum1,
            Enum2,
            Enum3
        }

        private NameValueCollection CreateSut()
        {
            var result = new NameValueCollection();
            result.Add("String1", "a string value");
            result.Add("String2", "");
            result.Add("String3", null);
            result.Add("Int1", "1");
            result.Add("Int2", "");
            result.Add("Long1", "1");
            result.Add("Long2", "");
            result.Add("Bool1", "true");
            result.Add("Bool2", "TRUE");
            result.Add("Bool3", "trUe");
            result.Add("Bool4", "false");
            result.Add("Bool5", "FALSE");
            result.Add("Bool6", "fAlse");
            result.Add("Bool7", "bla");
            result.Add("Bool8", "");
            result.Add("Bool9", null);
            result.Add("Enum1", "Enum1");
            result.Add("Enum2", "Enum2");
            result.Add("Enum3", "Enum3");
            result.Add("Enum1_int", "0");
            result.Add("Enum2_int", "1");
            result.Add("Enum3_int", "2");
            return result;
        }

        [Fact]
        public void GetStringWorks() {
            var sut = CreateSut();
            Assert.Equal("a string value", sut.GetString("String1"));
            Assert.Equal("", sut.GetString("String2"));
            Assert.Null(sut.GetString("String3"));
            Assert.Null(sut.GetString("String4"));
        }

        [Fact]
        public void GetIntWorks() {
            var sut = CreateSut();
            Assert.Equal(1,sut.GetInt("Int1"));
            try {
                Assert.Equal(0, sut.GetInt("Int2"));
                throw new Exception("FormatException expected");
            }
            catch (FormatException) {}
        }

        [Fact]
        public void GetLongWorks() {
            var sut = CreateSut();
            Assert.Equal(1L, sut.GetInt("Long1"));
            try
            {
                Assert.Equal(0, sut.GetInt("Long2"));
                throw new Exception("FormatException expected");
            }
            catch (FormatException) {}
        }

        [Fact]
        public void GetBoolWorks() {
            var sut = CreateSut();
            Assert.True(sut.GetBool("Bool1"));
            Assert.True(sut.GetBool("Bool2"));
            Assert.True(sut.GetBool("Bool3"));
            Assert.False(sut.GetBool("Bool4"));
            Assert.False(sut.GetBool("Bool5"));
            Assert.False(sut.GetBool("Bool6"));
            try
            {
                Assert.Equal(true, sut.GetBool("Bool7"));
                throw new Exception("FormatException expected");
            }
            catch (FormatException) { }
            try
            {
                Assert.Equal(true, sut.GetBool("Bool8"));
                throw new Exception("FormatException expected");
            }
            catch (FormatException) { }
            try
            {
                Assert.Equal(true, sut.GetBool("Bool9"));
                throw new Exception("ArgumentNullException expected");
            }
            catch (ArgumentNullException) { }
        }

        [Fact]
        public void GetEnumWorks() {
            var sut = CreateSut();
            Assert.Equal(SomeEnum.Enum1, sut.GetEnum<SomeEnum>("Enum1"));
            Assert.Equal(SomeEnum.Enum2, sut.GetEnum<SomeEnum>("Enum2"));
            Assert.Equal(SomeEnum.Enum3, sut.GetEnum<SomeEnum>("Enum3"));
            Assert.Equal(SomeEnum.Enum1, sut.GetEnum<SomeEnum>("Enum1_int"));
            Assert.Equal(SomeEnum.Enum2, sut.GetEnum<SomeEnum>("Enum2_int"));
            Assert.Equal(SomeEnum.Enum3, sut.GetEnum<SomeEnum>("Enum3_int"));

            Assert.Equal(SomeEnum.Enum1, sut.GetEnum("Enum1",typeof(SomeEnum)));
            Assert.Equal(SomeEnum.Enum2, sut.GetEnum("Enum2", typeof(SomeEnum)));
            Assert.Equal(SomeEnum.Enum3, sut.GetEnum("Enum3", typeof(SomeEnum)));
            Assert.Equal(SomeEnum.Enum1, sut.GetEnum("Enum1_int", typeof(SomeEnum)));
            Assert.Equal(SomeEnum.Enum2, sut.GetEnum("Enum2_int", typeof(SomeEnum)));
            Assert.Equal(SomeEnum.Enum3, sut.GetEnum("Enum3_int", typeof(SomeEnum)));
        }
    }
}