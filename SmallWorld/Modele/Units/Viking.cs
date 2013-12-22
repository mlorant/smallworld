using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class Viking : Unit, IViking
    {
        public Viking(int id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Compute how many points the units worth for
        /// the player.
        /// Viking: No points on water and sand and 1 additionnal 
        /// point if near water. (Otherwise, default points)
        /// </summary>
        /// <returns></returns>
        public override int getPoints()
        {
            ICase targetType = Game.Instance.Map.getCase(CurrentPosition);

            if (targetType is Sea || targetType is Desert)
                return 0;
            else if (this.isNearWater())
                return base.getPoints() + 1;
            else
                return base.getPoints();
        }

        private bool isNearWater()
        {

            // Make a list of all available points around the unit
            List<Point> points = new List<Point>();

            if (CurrentPosition.Y >= 1) // North
                points.Add(new Point(CurrentPosition.X, CurrentPosition.Y - 1));

            if (CurrentPosition.X >= 1) // West
                points.Add(new Point(CurrentPosition.X - 1, CurrentPosition.Y));

            if (CurrentPosition.X < Game.Instance.Map.Width - 1) // East
                points.Add(new Point(CurrentPosition.X + 1, CurrentPosition.Y));

            if (CurrentPosition.Y < Game.Instance.Map.Width - 1) // South
                points.Add(new Point(CurrentPosition.X, CurrentPosition.Y + 1));

            // Check for every point if it's a sea tile
            foreach (Point pt in points)
            {
                if (Game.Instance.Map.getCase(pt) is Sea)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Check if the unit can move to the position given,
        /// especially on a sea.
        /// </summary>
        /// <param name="tgt"></param>
        /// <returns></returns>
        public override bool canMoveOn(Point tgt)
        {
            if (this.MovePoint > 0)
            {
                return this.isNear(tgt);
            }
            return false;
        }
    }
}
