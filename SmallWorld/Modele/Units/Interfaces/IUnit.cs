using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IUnit
    {

        int Id { get; set; }
        int Health { get; set; }
        int Attack { get; }
        int Defense { get; }
        Point CurrentPosition { get; set; }
        bool attack(Point target);

        double computePercentageToWin(IUnit defender);

        bool isAlive();

        bool move(Point target);

        bool canMoveOn(Point target);
    }

    public interface IDwarf : IUnit    {}
    public interface IGallic : IUnit   {}
    public interface IViking : IUnit   {}
}
