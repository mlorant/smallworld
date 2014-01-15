using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using mapWrapper;
using System.Runtime.Serialization;

namespace SmallWorld
{
    /// <summary>
    /// Map creation and management. Store a grid of tile instance
    /// with a flyweight design pattern to optimize ressources.
    /// Units are located on the map via the tile index in the array.
    /// </summary>
    [Serializable()]
    public class Map : IMap, ISerializable
    {

        /// <summary>
        /// Map grid. Composed of instances of ICase, which inform
        /// about the type of terrain (plain, sea, forest, ...)
        /// 
        /// Each tile is represented with (x, y) coordinates, which
        /// can be computed with the width of the grid (the grid is 
        /// necessarely a square)
        /// </summary>
        private ICase[] _grid;

        /// <summary>
        /// Instances list of square types (Flyweight pattern)
        /// </summary>
        static Dictionary<int, ICase> _casesReferences = new Dictionary<int,ICase>();

        /// <summary>
        /// List of units present on each tiles
        /// </summary>
        private List<IUnit>[] _units;

        /// <summary>
        /// Width of the map
        /// </summary>
        private int _width;

        /// <summary>
        /// Engine which make suggestion of move for units
        /// </summary>
        private WrapperMapSuggestion _suggestEngine;


        /// <summary>
        /// Size of the map
        /// </summary>
        public int Size
        {
            get { return _grid.Length; }
        }

        /// <summary>
        /// Width of the map
        /// </summary>
        public int Width
        {
            get { return _width; }
        }

        /// <summary>
        /// Engine for the suggestion process. 
        /// </summary>
        public WrapperMapSuggestion SuggestEngine
        {
            get { return this._suggestEngine; }
        }

        /// <summary>
        /// Init a new map with a new flyweigh pattern
        /// </summary>
        static Map()
        {
            _casesReferences.Add(0, new Desert());
            _casesReferences.Add(1, new Forest());
            _casesReferences.Add(2, new Mountain());
            _casesReferences.Add(3, new Plain());
            _casesReferences.Add(4, new Sea());
        }

        /// <summary>
        /// Call the C++ library to generate a new random map
        /// with the width given. The new map will be a square,
        /// so the number of tiles will be mapWidth*mapWidth
        /// </summary>
        /// <param name="mapWidth">Width of the map generated</param>
        public void generateMap(int mapWidth)
        {
            this._width = mapWidth;

            int nbTiles = _width * _width;

            WrapperMapGenerator wrapper = new WrapperMapGenerator();
            List<int> cases = wrapper.generate_map(_width);

            _units = new List<IUnit>[nbTiles];
            _grid = new ICase[nbTiles];

            for (int i = 0; i < nbTiles; i++)
            {
                _grid[i] = this.getCaseTypeInstance(cases[i]);
            }

            _suggestEngine = new WrapperMapSuggestion(cases, _width);
        }

        /// <summary>
        /// Get the tile from the flyweight dictionnary if exists.
        /// If the index is unknown, return an exception.
        /// </summary>
        /// <param name="caseIndex">Tile index to return</param>
        /// <returns>Tile, if exists</returns>
        private ICase getCaseTypeInstance(int caseIndex)
        {
            if (!_casesReferences.ContainsKey(caseIndex))
            {
                throw new UnknownTileException();
            }

            return _casesReferences[caseIndex];
        }

        /// <summary>
        /// Get two points at the north-west and south-east of the map
        /// which aren't and the water. This points will be used to 
        /// determine the initial position of player units
        /// </summary>
        /// <returns>An array of 2 points at opposites of the map</returns>
        public Point[] getStartPoints()
        {
            Point[] points = new Point[2];
            bool found = false;

            // Get first player
            Point p = new Point(0, 0);
            while (!found)
            {
                if (!(this.getCase(p) is Sea))
                {
                    points[0] = p;
                    found = true;
                }
                else
                {
                    if (p.X > p.Y)
                        p.Y++;
                    else
                        p.X++;
                }
            }

           
            // Get second player
            p = new Point(this.Width-1, this.Width-1);
            found = false;
            while (!found)
            {
                if (!(this.getCase(p) is Sea))
                {
                    points[1] = p;
                    found = true;
                }
                else
                {
                    if (p.X > p.Y)
                        p.Y--;
                    else
                        p.X--;
                }
            }

            return points;
        }

        /// <summary>
        /// Return the index in the list for a coordinates given.
        /// Grid is stored in a 1-dimension list, so a point can be
        /// converted into the index by the formula `X*width + Y`.
        /// </summary>
        /// <param name="pos">Position to convert</param>
        /// <returns>Index in the grid computed</returns>
        private int getIndexFromPoint(Point pos)
        {
            int index = pos.X * _width + pos.Y;
            if (index > _grid.Length)
                throw new ArgumentException("Out of bounds: there's only " + _grid.Length + " tiles in the grid");

            return index;
        }


        /// <summary>
        /// Return the tile type of the position given
        /// </summary>
        /// <param name="pos">Position to retrieve</param>
        /// <returns>Tule instance requested</returns>
        public ICase getCase(Point pos)
        {
            int index = this.getIndexFromPoint(pos);
            return _grid[index];
        }

        /// <summary>
        /// Get list of units on a given tile
        /// </summary>
        /// <param name="pos">Position to fetch</param>
        /// <returns>List of units on the position requested</returns>
        public List<IUnit> getUnits(Point pos)
        {
            int index = this.getIndexFromPoint(pos);
            if (_units[index] == null)
                _units[index] = new List<IUnit>();
                
            return _units[index];
            
        }


        /// <summary>
        /// Place units at the beginning. Every units of the list
        /// will be place to the same position, given in the second
        /// argument.
        /// </summary>
        /// <param name="playerUnits">Units of the player to place</param>
        /// <param name="pos">Initial position of units</param>
        public void initUnits(List<IUnit> playerUnits, Point pos)
        {
            int index = getIndexFromPoint(pos);
            _units[index] = new List<IUnit>();
            foreach (IUnit unit in playerUnits)
            {
                _units[index].Add(unit);
            }
        }

        /// <summary>
        /// Returns the best defensive unit on the tile, which will
        /// be used to fight in the battle.
        /// </summary>
        /// <param name="tgt">Position (x, y) of the tile</param>
        /// <returns>The best defensive unit of the tile</returns>
        public IUnit getBestDefensiveUnit(Point tgt)
        {
            int index = this.getIndexFromPoint(tgt);

            IUnit bestUnit = null;
            foreach (IUnit unit in this._units[index])
            {
                if (bestUnit == null || bestUnit.Defense < unit.Defense)
                {
                    bestUnit = unit;
                }
            }

            return bestUnit;
        }

        // Serialization function
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("MapWidth", this._width);

            // Create an invert dict, to get the index of each case easily
            Dictionary<ICase, int> tilesIndex = new Dictionary<ICase, int>();
            foreach (KeyValuePair<int, ICase> pair in Map._casesReferences)
            {
                tilesIndex.Add(pair.Value, pair.Key);
            }

            int[] tiles = new int[this.Size];
            for(int i = 0; i < this.Size; i++) {
                tiles[i] = tilesIndex[this._grid[i]];
            }

            info.AddValue("MapGrid", tiles);
            info.AddValue("MapUnitsPosition", this._units);
            
        }

        /// <summary>
        /// Get a List of units position with int indexing instead
        /// of classes. Used for passing the map information to the
        /// C++ Wrapper.
        /// </summary>
        public List<int> NativeUnits
        {
            get
            {
                List<int> res = new List<int>();
                var revertReferences = _casesReferences.ToDictionary(x => x.Value, x => x.Key);
                
                for (int i = 0; i < _width * _width; i++)
                {
                    if (_units[i] == null || _units[i].Count == 0)
                        res.Add((int) UnitType.None);
                    else
                    {
                        UnitType unitEnum = (UnitType)Enum.Parse(typeof(UnitType), _units[i][0].GetType().Name, true);
                        res.Add((int) unitEnum);
                    }
                }

                return res;
            }
        }


        public Map() { }

        // Deserialization constructor.
        public Map(SerializationInfo info, StreamingContext ctxt)
        {
            this._width = (int) info.GetValue("MapWidth", typeof(int));
            this._units = (List<IUnit>[])info.GetValue("MapUnitsPosition", typeof(List<IUnit>[]));


            int[] tiles = (int[])info.GetValue("MapGrid", typeof(int[]));
            int nbTiles = _width * _width;

            this._grid = new ICase[nbTiles];
            for (int i = 0; i < nbTiles; i++)
                _grid[i] = _casesReferences[tiles[i]];

            // Recover map suggestion
            List<int> cases = new List<int>();
            var revertReferences = _casesReferences.ToDictionary(x => x.Value, x => x.Key);

            for (int i = 0; i < _width*_width; i++)
                cases.Add(revertReferences[_grid[i]]);

            _suggestEngine = new WrapperMapSuggestion(cases, _width);
        }
    }

    /// <summary>
    /// Exception raised when a unknown tile index is 
    /// requested.
    /// </summary>
    class UnknownTileException : Exception
    {
    }
}
