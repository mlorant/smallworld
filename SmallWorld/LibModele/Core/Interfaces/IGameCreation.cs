using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    /// <summary>
    /// Game creation factory interface.
    /// Allows to create game with the strategy pattern
    /// to choose the game mode
    /// </summary>
    public interface IGameCreation
    {

        /// <summary>
        /// Generic builder of a game, which initialize a map
        /// and game parameters
        /// </summary>
        /// <returns>A new instance of game created</returns>
        Game createGame(string player1, NationType nation1, string player2, NationType nation2);
    }
}
