using NUnit.Framework;

namespace TriangleTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            Assert.Multiple(() =>
            {
                Assert.IsTrue(true);
                Assert.IsTrue(false);
                Assert.IsTrue(false);
            });
        }
    }
}