using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class Dwarf : Unit, IDwarf
    {
        void IUnit.attack(ICase target)
        {
            throw new NotImplementedException();
        }

        bool IUnit.isAlive()
        {
            throw new NotImplementedException();
        }

        void IUnit.move(ICase target)
        {
            throw new NotImplementedException();
        }
    }
}
