using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallWorld;
using System.Drawing;

namespace UnitTestsSmallWorld
{
    /// <summary>
    /// Description résumée pour UnitTest
    /// </summary>
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestNewUnitLife()
        {
            Unit dwarf = new Dwarf();
            Assert.IsTrue(dwarf.isAlive());
            Assert.AreEqual(5, dwarf.Health);
        }

        [TestMethod]
        public void TestInitialStats()
        {
            Unit dwarf = new Dwarf();
            Assert.AreEqual(5, dwarf.Health);
            Assert.AreEqual(2, dwarf.Attack);
            Assert.AreEqual(1, dwarf.Defense);
        }

        [TestMethod]
        public void TestUnitDeath()
        {
            Unit dwarf = new Dwarf();
            dwarf.Health -= 5;
            Assert.IsFalse(dwarf.isAlive());
        } 
    }
}
