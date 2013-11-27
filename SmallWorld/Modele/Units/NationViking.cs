using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class NationViking : Nation, INationViking
    {
        public Viking fabricUnit()
        {
            throw new System.NotImplementedException();
        }

        IUnit INation.fabricUnit()
        {
            throw new NotImplementedException();
        }
    }
}
