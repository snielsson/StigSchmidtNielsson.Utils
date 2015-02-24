using StigSchmidtNielsson.Utils.Extensions;
using Xunit;
namespace StigSchmidtNielsson.Utils.Test.Extensions {
    public class ObjectExtensionsTests {
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


        [Fact]
        public void CloneWorks() {
            var someObj = new SomeClass {
                Id = 1,
                Name = "bla"
            };
            var someObjClone = someObj.Clone();
            Assert.False(someObj.Equals(someObjClone));
            Assert.False(ReferenceEquals(someObj, someObjClone));
        }
        [Fact]
        public void ToJsonWorks() {}
        [Fact]
        public void ToPrettyJsonWorks() {}
        [Fact]
        public void DumpWorks() {}
    }
}