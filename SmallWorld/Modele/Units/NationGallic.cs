using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class NationGallic : Nation, INationGallic
    {
        public Gallic fabricUnit()
        {
            throw new System.NotImplementedException();
        }

        IUnit INation.fabricUnit()
        {
            throw new NotImplementedException();
        }
    }
}
