using LinqHelper;

namespace LinqHelperTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var result=ItemGenerator.Create().CreateTree((t)=>t.Parernt is null,(t)=>t.Parernt)
        }
    }
}