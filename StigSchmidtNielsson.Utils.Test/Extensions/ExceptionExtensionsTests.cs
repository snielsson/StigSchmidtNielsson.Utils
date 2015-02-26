using System;
using StigSchmidtNielsson.Utils.Extensions;
using Xunit;
namespace StigSchmidtNielsson.Utils.Test.Extensions {
    public class ExceptionExtensionsTests {
        [Fact]
        public void ToStringRecursiveWorks() {
            try {
                throw new Exception("exception 1", new Exception("exception 2", new Exception("exception 3")));
            }
            catch (Exception ex) {
                var str = ex.ToStringRecursive();
                Assert.True(str.Matches("exception 1.*exception 2.*exception 3"));
                var str2 = ex.ToString();
                Assert.NotEqual(str, str2);
            }
        }
    }
}