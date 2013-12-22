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
            Unit dwarf = new Dwarf(0);
            Assert.IsTrue(dwarf.isAlive());
            Assert.AreEqual(5, dwarf.Health);
        }

        [TestMethod]
        public void TestInitialStats()
        {
            Unit dwarf = new Dwarf(0);
            Assert.AreEqual(5, dwarf.Health);
            Assert.AreEqual(2, dwarf.Attack);
            Assert.AreEqual(1, dwarf.Defense);
        }

        [TestMethod]
        public void TestUnitDeath()
        {
            Unit dwarf = new Dwarf(0);
            dwarf.Health -= 5;
            Assert.IsFalse(dwarf.isAlive());
        }

        [TestMethod]
        public void TestAttackPoint()
        {
            IUnit u = new Dwarf(0);
            int attackPoint = 2;
            int healthPoint = 5;
            const int MAXHEALTH = 5;            

            Assert.AreEqual(u.Attack, attackPoint);

            u.Health = 0;
            Assert.AreEqual(u.Attack, 0);

            u.Health = 3;
            healthPoint = 3;
            int attackPointWithLife = (int)Math.Round(attackPoint * 
                ((double)healthPoint / MAXHEALTH), 0, MidpointRounding.AwayFromZero);
            Assert.IsTrue(u.Health == 3);
            Assert.AreEqual(u.Attack, attackPointWithLife);
        }

        [TestMethod]
        public void TestPercentageOfWin()
        {
            IUnit u = new Viking(0);
            IUnit d = new Dwarf(1);
            // attack = 2, defense = 1
            double initialPercentgage = 25;
            double percentageToTestU = u.computePercentageToWin(d);
            Assert.AreEqual(initialPercentgage, percentageToTestU);

            u.Health = 2;
            percentageToTestU = u.computePercentageToWin(d);
            double percentageToTestD = d.computePercentageToWin(u);
            Assert.IsTrue(percentageToTestU > percentageToTestD);
            Assert.AreEqual(u.Attack, 1);
            Assert.AreEqual(percentageToTestU, 50);
        }
    }
}
