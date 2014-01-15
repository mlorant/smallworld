using mapWrapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SmallWorld
{

    /// <summary>
    /// Representation of a map in the game.
    /// A map is compose of a squared grid of 
    /// tiles and units in tiles. Several units 
    /// can be on a single tile.
    /// </summary>
    public interface IMap
    {

        /// <summary>
        /// Width of the map (not to be confused with its total size)
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Number total of tiles on the map. 
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Engine for the suggestion process. 
        /// </summary>
        WrapperMapSuggestion SuggestEngine { get; }

        /// <summary>
        /// Get a List of units position with int indexing instead
        /// of classes. Used for passing the map information to the
        /// C++ Wrapper.
        /// </summary>
        List<int> NativeUnits { get; }

        /// <summary>
        /// Call the C++ library to generate a new random map
        /// with the width given. The new map will be a square,
        /// so the number of tiles will be mapWidth*mapWidth
        /// </summary>
        /// <param name="mapWidth">Width of the map generated</param>
        void generateMap(int mapWidth);

        /// <summary>
        /// Return the tile type of the position given
        /// </summary>
        /// <param name="pos">Position to retrieve</param>
        /// <returns>Tule instance requested</returns>
        ICase getCase(Point pos);

        /// <summary>
        /// Get two points at the north-west and south-east of the map
        /// which aren't and the water. This points will be used to 
        /// determine the initial position of player units
        /// </summary>
        /// <returns>An array of 2 points at opposites of the map</returns>
        Point[] getStartPoints();

        /// <summary>
        /// Place units at the beginning. Every units of the list
        /// will be place to the same position, given in the second
        /// argument.
        /// </summary>
        /// <param name="playerUnits">Units of the player to place</param>
        /// <param name="pos">Initial position of units</param>
        void initUnits(List<IUnit> playerUnits, Point pos);

        /// <summary>
        /// Get list of units on a given tile
        /// </summary>
        /// <param name="pos">Position to fetch</param>
        /// <returns>List of units on the position requested</returns>
        List<IUnit> getUnits(Point pos);

        /// <summary>
        /// Returns the best defensive unit on the tile, which will
        /// be used to fight in the battle.
        /// </summary>
        /// <param name="index">Index of the tile</param>
        /// <returns>The best defensive unit of the tile</returns>
        IUnit getBestDefensiveUnit(Point pos);
    }
}
