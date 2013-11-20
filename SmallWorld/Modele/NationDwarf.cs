using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class NationDwarf : Nation, INationDwarf
    {
        public Dwarf fabricUnit()
        {
            throw new System.NotImplementedException();
        }

        IUnit INation.fabricUnit()
        {
            throw new NotImplementedException();
        }
    }
}
