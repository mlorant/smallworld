using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public enum NationType { VIKING, DWARF, GALLIC };

    public abstract class Nation : INation
    {
        public abstract IUnit fabricUnit(int id);

        public abstract bool hasUnit(IUnit unit);
    }
}
