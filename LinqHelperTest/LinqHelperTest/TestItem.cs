using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqHelperTest
{
    public class TestItem
    {
        public TestItem? Parernt { get; set; }

        public string Name { get; set; } = "";

        public TestItem[] Children { get; set; }=Array.Empty<TestItem>();
    }
}
