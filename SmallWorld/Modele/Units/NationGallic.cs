using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class NationGallic : Nation, INationGallic
    {
        public IGallic fabricUnit()
        {
            return new Gallic();
        }
    }
}
