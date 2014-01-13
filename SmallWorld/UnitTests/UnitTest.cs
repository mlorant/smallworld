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

        [TestMethod]
        public void TestBuryUnit()
        {
            Game.Instance.initGame(2, 3);
            Game.Instance.initPlayer(0, "A", NationType.DWARF);
            Game.Instance.initPlayer(1, "B", NationType.GALLIC);
            Game.Instance.createMap(3);
            Game.Instance.Map.initUnits(Game.Instance.Players[0].Units, new Point(0, 0));
            Game.Instance.Map.initUnits(Game.Instance.Players[1].Units, new Point(2, 2));
            int j = 0;
            while (j < Game.Instance.Players[0].Units.Count)
            {
                Game.Instance.Players[0].Units[j].CurrentPosition = new Point(0, 0);
                Game.Instance.Players[1].Units[j].CurrentPosition = new Point(2, 2);
                j++;
            }
            while(Game.Instance.Players[0].Units.Count != 0)
            {
                IUnit toBury = Game.Instance.Players[0].Units[0];
                toBury.Health = 0;
                Assert.IsFalse(toBury.isAlive());

                toBury.buryUnit(Game.Instance.Players[0], toBury.CurrentPosition);
            };
            Assert.AreEqual(0, Game.Instance.Players[0].Units.Count);
            Assert.AreEqual(0, Game.Instance.Map.getUnits(new Point(0, 0)).Count);


            IPlayer p2 = Game.Instance.Players[1];
            Assert.AreEqual(3, p2.Units.Count);
            Assert.AreEqual(3, Game.Instance.Map.getUnits(new Point(2, 2)).Count);
            
            p2.Units[0].buryUnit(p2, p2.Units[0].CurrentPosition);
            
            Assert.AreEqual(2, p2.Units.Count);
            Assert.AreEqual(2, Game.Instance.Map.getUnits(new Point(2, 2)).Count);

            // 1 because n°0 is delete
            p2.Units[1].buryUnit(p2, new Point(2, 2));
            Assert.AreEqual(1, Game.Instance.Map.getUnits(new Point(2, 2)).Count);
            Assert.AreEqual(1, p2.Units.Count);

            Game.Instance.Map.getUnits(new Point(2,2))[0].buryUnit(p2, new Point(2, 2));
            Assert.AreEqual(0, Game.Instance.Map.getUnits(new Point(2, 2)).Count);
            Assert.AreEqual(0, p2.Units.Count);
        }

    }
}
