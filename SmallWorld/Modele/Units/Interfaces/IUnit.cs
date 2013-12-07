using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IUnit
    {

        int Health { get; set; }
        int Attack { get; }
        int Defense { get; }
        

        void attack(ICase target);

        bool isAlive();

        void move(ICase target);
    }

    public interface IDwarf : IUnit    {}
    public interface IGallic : IUnit   {}
    public interface IViking : IUnit   {}
}
