using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class NationDwarf : Nation, INationDwarf
    {
        public override IUnit fabricUnit(int id)
        {
            return new Dwarf(id);
        }
    }
}
