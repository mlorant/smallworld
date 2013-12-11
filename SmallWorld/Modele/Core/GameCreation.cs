using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    /// <summary>
    /// Enum of game mode possible
    /// </summary>
    public enum GameSize { DEMO, SMALL, NORMAL };

    /// <summary>
    /// Base class for the game builder, with a generic
    /// behavior
    /// </summary>
    public abstract class GameCreation : IGameCreation
    {

        /// <summary>
        /// Generic builder of a game, which initialize a map
        /// and game parameters
        /// </summary>
        /// <returns>A new instance of game created</returns>
        public Game createGame(string player1, NationType nation1, string player2, NationType nation2)
        {
            Game g = Game.Instance;
            g.initGame(this.NB_ROUNDS, this.NB_UNITS);
            g.createMap(MAP_SIZE);

            g.initPlayer(0, player1, nation1);
            g.initPlayer(1, player2, nation2);

            g.placePlayers();

            return g;
        }
        

        /// <summary>
        /// Maximal number of round possible in a game.
        /// Will be override by subclasses
        /// </summary>
        public abstract int NB_ROUNDS 
        {
            get; 
        }

        /// <summary>
        /// Side size of the map (in number of tiles)
        /// Will be override by subclasses
        /// </summary>
        public abstract int MAP_SIZE
        {
            get;
        }

        /// <summary>
        /// Number of the units for each player at the beginning of the game.
        /// Will be override by subclasses
        /// </summary>
        public abstract int NB_UNITS
        {
            get;
        }
    }
}
