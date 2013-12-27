using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IUnit
    {

        String Name { get; }
        int Id { get; set; }
        int Health { get; set; }
        int MaxHealth { get; }
        int Attack { get; }
        int Defense { get; }
        double MovePoint { get; }
        double MaxMovePoint { get; }

        Point CurrentPosition { get; set; }
        bool attack(IUnit defender, Point target);

        void buryUnit(IPlayer general, Point assaultLocation);

        double computePercentageToWin(IUnit defender);

        bool isAlive();

        bool move(Point target);

        bool canMoveOn(Point target);
    }

    public interface IDwarf : IUnit    {}
    public interface IGallic : IUnit   {}
    public interface IViking : IUnit   {}
}
