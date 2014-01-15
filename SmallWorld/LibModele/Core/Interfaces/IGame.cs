using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{

    /// <summary>
    /// Core class of SmallWorld : directs the
    /// game, and let play each player when it's
    /// their turn.
    /// </summary>
    public interface IGame
    {

        /// <summary>
        /// Init a player according to its configuration
        /// </summary>
        /// <param name="i">Player number</param>
        /// <param name="nickname">Personal nickname</param>
        /// <param name="nation">Nation choosed</param>
        void initPlayer(int i, string nickname, NationType nation);

        /// <summary>
        /// Generate a new random map with the size defined
        /// by the game type
        /// </summary>
        void createMap(int mapSize);

        /// <summary>
        /// Place units players as far as possible on the game board
        /// </summary>
        void placePlayers();

        /// <summary>
        /// Determine who's playing first
        /// </summary>
        void determinePlayOrder();

        /// <summary>
        /// Check if the game is finished or not, according to
        /// the round number and units remaining
        /// </summary>
        /// <returns>A boolean (true if the game is finished)</returns>
        bool Finished { get; }

        /// <summary>
        /// Return the player who wins the current game, by computing
        /// points for each players
        /// </summary>
        IPlayer Winner { get; }
    }
}
