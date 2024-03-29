﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallWorld;

namespace UnitTestsSmallWorld
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void TestCreatePlayer()
        {
            Player p = new Player("Maxime", NationType.GALLIC, 5);
            
            Assert.AreEqual(p.Nickname, "Maxime");
            Assert.IsTrue(p.Units.Count == 5);
            for (int i = 0; i < p.Units.Count; i++)
            {
                Assert.IsTrue(p.Units[i] is Gallic);
            }
        }
    }
}
