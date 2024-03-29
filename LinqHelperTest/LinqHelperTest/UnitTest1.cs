using LinqHelper;

namespace LinqHelperTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var result = ItemGenerator.
                Create().
                NotNull().
                CreateTree(
                    (item) => item.Parernt is null,
                    //(parent) => (child) => parent.Parernt is not null && parent.Parernt.Equals(child),
                    (parent) => (child) => child.Parernt is not null && child.Parernt.Equals(parent),
                    (parent, children) => parent.Children = children.ToArray()
                );
            var test= result.ToArray();
            Assert.IsNotNull(test);

            var items = ItemGenerator.Create();
            var path = LambdaExtension.GetPropertyPath<TestItem>(t => t.Name);
            Assert.IsNotNull(path);
            var val = items.First().GetPropertyValueFromPath(path);
            Assert.IsNotNull(val);
            items.First().SetPropertyValueFromPath(path, val);
        }
    }
}