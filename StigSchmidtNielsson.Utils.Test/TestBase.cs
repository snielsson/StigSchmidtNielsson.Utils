using System.Collections.Generic;
using Ploeh.AutoFixture;
namespace StigSchmidtNielsson.Utils.Test {
    public class TestBase {
        protected Fixture Fixture { get; set; }
        public TestBase() {
            Fixture = new Fixture();
        }
        protected T Create<T>() {
            return Fixture.Create<T>();
        }

        protected IEnumerable<T> CreateMany<T>() {
            return Fixture.CreateMany<T>();
        }
    }
}