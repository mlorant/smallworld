using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IUnit
    {

        string Name { get; }
        int Id { get; set; }
        int Health { get; set; }
        int MaxHealth { get; }
        int Attack { get; }
        int Defense { get; }
        double MovePoint { get; }
        double MaxMovePoint { get; }

        Point CurrentPosition { get; set; }

        bool attack(IUnit defender, Point target);

        double computePercentageToWin(IUnit defender);

        bool isAlive();

        bool move(Point target);

        bool canMoveOn(Point target);

        List<Tuple<Point, MoveType>> getSuggestedPoints();
    }

    public interface IDwarf : IUnit    {}
    public interface IGallic : IUnit   {}
    public interface IViking : IUnit   {}
}
