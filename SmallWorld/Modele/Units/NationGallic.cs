using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class NationGallic : Nation, INationGallic
    {
        public override IUnit fabricUnit(int id)
        {
            return new Gallic(id);
        }

        public override bool hasUnit(IUnit unit)
        {
            return unit is Gallic;
        }
    }
}
