using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public abstract class Nation : INation
    {
        public IUnit fabricUnit()
        {
            throw new System.NotImplementedException();
        }
    }
}
