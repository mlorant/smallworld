using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SmallWorld
{

    /// <summary>
    /// Base interface for unit. An unit has a name, 
    /// an health status and attack/defense points, compute
    /// according to the attacker/defender.
    /// Each unit known it own position. 
    /// </summary>
    public interface IUnit
    {
        /// <summary>Name of the unit</summary>
        string Name { get; }
        /// <summary>Unique identifier for the unit (used in the name)</summary>
        int Id { get; set; }
        /// <summary>Current Health</summary>
        int Health { get; set; }
        /// <summary>Max Health at the begin of the game</summary>
        int MaxHealth { get; }
        /// <summary>Base attack points</summary>
        int Attack { get; }
        /// <summary>Base defense points</summary>
        int Defense { get; }
        /// <summary>Current move points available</summary>
        double MovePoint { get; }
        /// <summary>Max move points possible in a round</summary>
        double MaxMovePoint { get; }
        /// <summary>Position on the map</summary>
        Point CurrentPosition { get; set; }

        /// <summary>Attack another unit located on the `target` point.
        /// Returns true if the current unit wins</summary>
        bool attack(IUnit defender, Point target);

        /// <summary>
        /// Get percentage of chance to win the battle
        /// against the defender unit given
        /// </summary>
        /// <param name="defender">Enemy unit</param>
        double computePercentageToWin(IUnit defender);

        /// <summary>
        /// Check if the unit is still alive
        /// </summary>
        /// <returns></returns>
        bool isAlive();

        /// <summary>
        /// Move the unit to the targeted point
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        bool move(Point target);

        /// <summary>
        /// Check if the unit can move on the desired point
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        bool canMoveOn(Point target);

        /// <summary>
        /// Returns a list of points where the units can go on the map
        /// </summary>
        /// <returns></returns>
        List<Tuple<Point, MoveType>> getSuggestedPoints();
    }

    /// <summary>
    /// Dwarf unit representation.
    /// Can move on any mountain without ennemies if he's 
    /// already on a mountain
    /// </summary>
    public interface IDwarf : IUnit    {}

    /// <summary>
    /// Gallic unit representation.
    /// Move faster on plains
    /// </summary>
    public interface IGallic : IUnit   {}

    /// <summary>
    /// Viking unit representation.
    /// Can move on sea
    /// </summary>
    public interface IViking : IUnit   {}
}
