using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{

    /// <summary>
    /// Representation of a player in the SmallWorld game.
    /// A player has a nickname and a list of units associated
    /// to one nation.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>Player nickname</summary>
        string Nickname { get; }

        /// <summary>Units list of the player</summary>
        List<IUnit> Units { get; }

        /// <summary>Player's nation. Use to fabric units</summary>
        INation Nation { get; }

        /// <summary>
        /// Compute and returns the number of points of the player,
        /// by suming every points of his units.
        /// </summary>
        /// <returns>The score of the player</returns>
        int computePoints();

        /// <summary>
        /// Returns the number of units the player can still
        /// play in the round
        /// </summary>
        int getNbUnitsToPlay();

        /// <summary>
        /// Delete the unit of the player list and the map
        /// </summary>
        /// <param name="general">Player of the unit dead</param>
        void buryUnit(IUnit unit);

    }
}
