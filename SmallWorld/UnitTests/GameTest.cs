using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallWorld;

namespace UnitTestsSmallWorld
{
    [TestClass]
    public class GameTest
    {
        Game game;

        [TestInitialize]
        public void Initialize()
        {
            game = Game.Instance;
            game.initGame(10, 10);
        }

        [TestMethod]
        public void TestCreationGame()
        {
            Assert.AreEqual(2, game.Players.Length);
            Assert.AreEqual(1, game.CurrentRound);
        }

        [TestMethod]
        public void TestIncrementRound()
        {
            Game game = Game.Instance;
            Assert.AreEqual(1, game.CurrentRound);

            game.CurrentRound += 1;
            Assert.AreEqual(2, game.CurrentRound);

            try
            {
                game.CurrentRound += 5;
                Assert.Fail("Exception should be raised (currentRound can only be incremented by 1");
            }
            catch (Exception) { }
        
        }
    }
}
