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

        public Map()
        {
            WrapperMapGenerator wrapper = new WrapperMapGenerator();
            List<int> cases = wrapper.generate_map(20);
            
            Console.Write(cases.ToString());
        }

        public ICase getCase()
        {
            throw new NotImplementedException();
        }

        public int getSize()
        {
            throw new NotImplementedException();
        }
    }
}
