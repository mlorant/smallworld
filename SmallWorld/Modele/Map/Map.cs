using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mapWrapper;

namespace SmallWorld
{
    /// <summary>
    /// Map creation and management. Store a grid of tile instance
    /// with a flyweight design pattern to optimize ressources.
    /// Units are located on the map via the tile index in the array.
    /// </summary>
    public class Map : IMap
    {

        /// <summary>
        /// Map grid. Composed of instances of ICase, which inform
        /// about the type of terrain (plain, sea, forest, ...)
        /// 
        /// Each tile is represented with (x, y) coordinates, which
        /// can be computed with the width of the grid (the grid is 
        /// necessarely a square)
        /// </summary>
        private ICase[] grid;

        /// <summary>
        /// Instances list of square types (Flyweight pattern)
        /// </summary>
        private ICase[] casesReferences;

        /// <summary>
        /// List of units present on each tiles
        /// </summary>
        private List<IUnit>[] units;

        /// <summary>
        /// Width of the map
        /// </summary>
        private int width;

        /// <summary>
        /// Size of the map
        /// </summary>
        public int Size
        {
            get { return grid.Length; }
        }

        /// <summary>
        /// Width of the map
        /// </summary>
        public int Width
        {
            get { return width; }
        }


        /// <summary>
        /// Init a new map with a new flyweigh pattern
        /// </summary>
        public Map()
        {
            casesReferences = new ICase[5];
        }


        public void generateMap(int mapWidth)
        {
            this.width = mapWidth;

            WrapperMapGenerator wrapper = new WrapperMapGenerator();
            List<int> cases = wrapper.generate_map(width);

            grid = new ICase[width*width];

            for (int i = 0; i < width*width; i++)
            {
                grid[i] = this.getCaseTypeInstance(cases[i]);
            }

            
        }


        public ICase getCaseTypeInstance(int caseIndex)
        {
            if (casesReferences[caseIndex] == null)
            {
                casesReferences[caseIndex] = generateCase(caseIndex);
            }

            return casesReferences[caseIndex];
        }

        /// <summary>
        /// Create a new instance of case
        /// </summary>
        /// <param name="caseIndex"></param>
        /// <returns></returns>
        private ICase generateCase(int caseIndex) 
        {
            ICase obj = null;
            switch (caseIndex)
            {
                case 0:
                    obj = new Desert();
                    break;
                case 1:
                    obj = new Forest();
                    break;
                case 2:
                    obj = new Mountain();
                    break;
                case 3:
                    obj = new Plain();
                    break;
                case 4:
                    obj = new Sea();
                    break;
                default:
                    throw new UnknownTileException();
            }
            
            return obj;
        }

        public ICase getCase(Point pos)
        {
            int index = pos.X * width + pos.Y;
            if (index > grid.Length)
            {
                throw new IndexOutOfRangeException("Out of bounds: there's only " + grid.Length + " tiles in the grid");
            }

            return grid[index];
        }

        /// <summary>
        /// Returns the best defensive unit on the tile, which will
        /// be used to fight in the battle.
        /// </summary>
        /// <param name="index">Position (x, y) of the tile</param>
        /// <returns>The best defensive unit of the tile</returns>
        public IUnit getBestDefensiveUnit(Point tgt)
        {
            int index = tgt.X * width + tgt.Y;
            if (index > grid.Length)
            {
                throw new IndexOutOfRangeException("Out of bounds: there's only " + grid.Length + " tiles in the grid");
            }

            IUnit bestUnit = null;

            foreach (IUnit unit in this.units[index])
            {
                if (bestUnit == null || bestUnit.Defense < unit.Defense)
                {
                    bestUnit = unit;
                }
            }

            return bestUnit;
        }
    }

    class UnknownTileException : Exception
    {
    }
}
