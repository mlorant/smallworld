using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallWorld;

namespace UnitTestsSmallWorld
{
    /// <summary>
    /// Description résumée pour NationTest
    /// </summary>
    [TestClass]
    public class NationTest
    {

        [TestMethod]
        public void TestFabricDwarfUnit()
        {
            Nation dwarfs = new NationDwarf();
            Assert.IsTrue(dwarfs.fabricUnit(0) is Dwarf);
        }

        [TestMethod]
        public void TestFabricGallicUnit()
        {
            Nation gallics = new NationGallic();
            Assert.IsTrue(gallics.fabricUnit(0) is Gallic);
        }

        [TestMethod]
        public void TestFabricVikingUnit()
        {
            Nation vikings = new NationViking();
            Assert.IsTrue(vikings.fabricUnit(0) is Viking);
        }
    }
}
