using System;
using System.Reflection;
using StigSchmidtNielsson.Utils.Extensions;
using Xunit;
namespace StigSchmidtNielsson.Utils.Test.Extensions {
    public class AssemblyExtensionsTests {
        [Fact]
        public void VersionStringWorks() {
            var sut = Assembly.GetExecutingAssembly();
            var versionString = sut.VersionString();
            Assert.Equal("1.0.0.0", versionString); // "1.0.0.0" is the version from the AssembleInfo of the current test assembly.
        }

        [Fact]
        public void BuildTimeWorks() {
            var sut = Assembly.GetExecutingAssembly();
            var buildTime = sut.BuildTime();
            Assert.Equal(DateTime.Today, buildTime.Date); // Assumes the current test assembly is built today.
        }

        [Fact]
        public void GetAssemblyInfoWorks() {
            var sut = Assembly.GetExecutingAssembly();
            var info = sut.GetAssemblyInfo();
            Assert.True(info.Matches(@"Assembly: .*, version= \d+\.\d+\.\d+\.\d+, buildtime= \d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d UTC"));
        }
    }
}