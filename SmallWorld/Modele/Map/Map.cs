using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mapWrapper;

namespace SmallWorld
{

    public class Map : IMap
    {
        private ICase[] grid;

        private ICase[] casesReferences;

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
                    // TODO: Add exception here
                    break;
            }
            
            return obj;
        }

        public int getSize()
        {
            return grid.Length;
        }

        public ICase getCase(int i)
        {
            // TODO : Add exception if i is too large
            return grid[i];
        }
    }
}
