using System;
using System.Collections.Generic;
using mapWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallWorld;

namespace UnitTestsSmallWorld
{
    [TestClass]
    public class WrapperTest
    {

        [TestMethod]
        public void TestGenerateMap()
        {
            WrapperMapGenerator wrapper = new WrapperMapGenerator();
            List<int> cases = wrapper.generate_map(10);
            Assert.AreEqual(cases.Count, 100);
            foreach (int c in cases)
            {
                Assert.IsTrue(c >= 0 && c < 5);
            }
        }
    }
}
