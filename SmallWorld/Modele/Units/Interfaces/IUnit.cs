using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IUnit
    {

        int Health { get; set; }
        int Attack { get; }
        int Defense { get; }
        Point CurrentPosition { get; set; }
        

        void attack(Point target);

        bool isAlive();

        void move(Point target);

        bool canMoveOn(Point target);
    }

    public interface IDwarf : IUnit    {}
    public interface IGallic : IUnit   {}
    public interface IViking : IUnit   {}
}
