using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqHelperTest
{
    public static class ItemGenerator
    {
        public static TestItem[] Create()
        {
            var test1 = new TestItem { Name = "1" };
            var test2 = new TestItem { Name = "2" };
            var test3 = new TestItem { Name = "3" };
            var test4 = new TestItem { Name = "1-1", Parernt = test1 };
            var test5 = new TestItem { Name = "1-2", Parernt = test1 };
            var test6 = new TestItem { Name = "3-1", Parernt = test3 };
            var test7 = new TestItem { Name = "3-1-1", Parernt = test6 };
            var test8 = new TestItem { Name = "3-1-2", Parernt = test6 };
            test1.Children = new[] { test4, test5 };
            test3.Children = new[] { test6 };
            return new[] { test1, test2, test3, test4, test5, test6, test7, test8 };
        }
    }
}
