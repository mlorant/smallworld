using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IUnit
    {

        void attack(ICase target);

        bool isAlive();

        void move(ICase target);
    }
}
