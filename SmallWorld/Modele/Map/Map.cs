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
        /// </summary>
        private ICase[] grid;

        /// <summary>
        /// Instances list of square types (Flyweight pattern)
        /// </summary>
        private ICase[] casesReferences;

        /// <summary>
        /// Size of the map
        /// </summary>
        public int Size
        {
            get { return grid.Length; }
        }

        /// <summary>
        /// Init a new map with a new flyweigh pattern
        /// </summary>
        public Map()
        {
            casesReferences = new ICase[5];
        }


        public void generateMap(int mapSize)
        {
            WrapperMapGenerator wrapper = new WrapperMapGenerator();
            List<int> cases = wrapper.generate_map(mapSize);

            grid = new ICase[mapSize];

            for (int i = 0; i < mapSize; i++)
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

        public ICase getCase(int i)
        {
            // TODO : Add exception if i is too large
            if (i > grid.Length)
            {
                throw new IndexOutOfRangeException("Out of bounds: there's only " + grid.Length + " tiles in the grid");
            }

            return grid[i];
        }
    }

    class UnknownTileException : Exception
    {
    }
}
