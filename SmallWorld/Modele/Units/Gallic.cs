using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class Gallic : Unit, IGallic
    {

        /// <summary>
        /// Check if the unit can move to the position given.
        /// Gallics can move on 2 Plain tile in 1 round.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool canMoveOn(Point tgt)
        {
            // Check if the move implies Plain, the speciality of
            // Gallics
            ICase destType = Game.Instance.Map.getCase(tgt);
            if (destType is Plain)
            {
                // Pythagoras theorem for the distance between 2 points
                int xSquared = (CurrentPosition.X - tgt.X) ^ 2;
                int ySquared = (CurrentPosition.Y - tgt.Y) ^ 2;
                if(xSquared + ySquared <= 2)
                    return true;
            }

            // If nothing happened here, check the parent method
            return base.canMoveOn(tgt);
        }

    }
}
