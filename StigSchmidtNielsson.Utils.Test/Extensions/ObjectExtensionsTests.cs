using System.IO;
using System.Text;
using StigSchmidtNielsson.Utils.Extensions;
using Xunit;
namespace StigSchmidtNielsson.Utils.Test.Extensions {
    public class ObjectExtensionsTests : TestBase {
        private class A {
            public B B { get; set; }
            public C C { get; set; }
        }

        private class B {
            public B() {
                NestedInBProp = new NestedInB();
            }
            public NestedInB NestedInBProp { get; set; }

            public class NestedInB {
                public string Name { get; set; }
            }
        }

        private class C {
            public A A { get; set; }
        }

        private class DerivedFromA : A {
            public string Name { get; set; }
        }

        private struct SomeStruct {
            private string _name;
            public SomeStruct(string name) {
                _name = name;
            }
            public string Name { get { return _name; } private set { _name = value; } }
        }

        private class SomeClass {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private DerivedFromA CreateA() {
            var a = new DerivedFromA();
            a.Name = Create<string>();
            a.B = new B {
                NestedInBProp = {
                    Name = Create<string>()
                }
            };
            a.C = new C();
            return a;
        }

        [Fact]
        public void CloneWorks() {
            var someObj = new SomeClass {
                Id = 1,
                Name = "bla"
            };
            var someObjClone = someObj.Clone();
            Assert.False(someObj.Equals(someObjClone));
            Assert.False(ReferenceEquals(someObj, someObjClone));

            var a = CreateA();
            var aClone = a.Clone();
            Assert.False(ReferenceEquals(a, aClone));
            var aJson = a.ToPrettyJson();
            var aCloneJson = aClone.ToPrettyJson();
            Assert.Equal(aJson, aCloneJson);
        }

        [Fact]
        public void ToJsonWorks() {
            var a = CreateA();
            var json = a.ToJson();
            Assert.True(json.Contains(a.Name));
            Assert.True(json.Contains(a.B.NestedInBProp.Name));
        }
        [Fact]
        public void ToPrettyJsonWorks() {
            var a = CreateA();
            var json = a.ToPrettyJson();
            Assert.True(json.Contains(a.Name));
            Assert.True(json.Contains(a.B.NestedInBProp.Name));
        }
        [Fact]
        public void DumpWorks() {
            var sb = new StringBuilder();
            TextWriter textWriter = new StringWriter(sb);
            var a = CreateA();
            a.Dump(textWriter);
            var str = sb.ToString();
            Assert.True(str.Contains(a.Name));
        }
    }
}