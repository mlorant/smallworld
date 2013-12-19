using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallWorld;

namespace UnitTestsSmallWorld
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void TestCreationGame()
        {
            Game game = Game.Instance;
            Assert.IsFalse(game.isGameFinished());
            Assert.AreEqual(game.Players.Length, 2);
            Assert.AreEqual(game.CurrentRound, 1);
        }

        [TestMethod]
        public void TestIncrementRound()
        {
            Game game = Game.Instance;
            Assert.AreEqual(game.CurrentRound, 1);

            game.CurrentRound += 1;
            Assert.AreEqual(game.CurrentRound, 2);

            try
            {
                game.CurrentRound += 5;
                Assert.Fail("Exception should be raised (currentRound can only be incremented by 1");
            }
            catch (Exception) { }
        
        }
    }
}
