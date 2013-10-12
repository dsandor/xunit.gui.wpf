using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace xunit.gui.wpf.tests
{
    public class RunnerViewModelTests
    {
        [Fact, Trait("Category", "First")]
        public void Test1()
        {
            Thread.Sleep(1000);
        }

        [Fact, Trait("Category", "First")]
        public void Test2()
        {
            Thread.Sleep(1000);
        }

        [Fact, Trait("Category", "First")]
        public void Test3()
        {
            Thread.Sleep(1000);
        }

        [Fact, Trait("Category", "First")]
        public void Test4()
        {
            Thread.Sleep(1000);
        }
    }

    public class OtherTests
    {
        [Fact, Trait("Category", "First")]
        public void OtherTest1()
        {
            Thread.Sleep(1000);
        }

        [Fact, Trait("Category", "First")]
        public void OtherTest2()
        {
            Thread.Sleep(1000);
        }

        [Fact, Trait("Category", "First")]
        public void OtherTest3()
        {
            Thread.Sleep(1000);
        }

        [Fact, Trait("Category", "First")]
        public void OtherTest4()
        {
            Thread.Sleep(1000);
        }
    }
}
