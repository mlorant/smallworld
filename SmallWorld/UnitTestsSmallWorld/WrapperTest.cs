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
        public void TestMapGeneratedSize()
        {
            WrapperMapGenerator wrapper = new WrapperMapGenerator();
            List<int> cases = wrapper.generate_map(10);
            Assert.AreEqual(cases.Count, 100);
            foreach (int c in cases)
            {
                Console.Write(c);
                Assert.IsTrue(c >= 0 && c < 5);
            }
        }

        [TestMethod]
        public void TestCasesRepresented()
        {
            WrapperMapGenerator wrapper = new WrapperMapGenerator();

            // Test 10000 times to be sure
            for (int i = 0; i < 10000; i++)
            {
                List<int> cases = wrapper.generate_map(10);

                bool[] typesPresent = { false, false, false, false, false };
                foreach (int c in cases)
                {
                    typesPresent[c] = true;
                }

                // Check if every bool has been set to True
                for (int j = 0; j < 5; j++)
                {
                    Assert.IsTrue(typesPresent[j]);
                }
            }
        }
    }
}
