using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class Dwarf : Unit, IDwarf
    {

        public Dwarf(int id)
        {
            this.Id = id;
        }


        /// <summary>
        /// Check if the unit can move to the position given.
        /// Dwarves can move on any mountain without ennemies if
        /// they're on a mountain already.
        /// </summary>
        /// <param name="tgt"></param>
        /// <returns></returns>

        public override bool canMoveOn(Point tgt)
        {
            ICase destType = Game.Instance.Map.getCase(tgt);
            ICase currentType = Game.Instance.Map.getCase(this.CurrentPosition);

            if(currentType is Mountain && destType is Mountain) 
            {
                // Check if there's unit of the other player
                if(Game.Instance.Map.getUnits(tgt).Count != 0)
                {
                    if (Game.Instance.Map.getUnits(tgt)[0].GetType() != this.GetType()) 
                    {
                        // unit of different type, so can't go on it,
                        // unless the base movement is allowed (to attack)
                        return false; 
                    }
                }

                return true;
            }

            // If nothing happened here, check the parent method
            return base.canMoveOn(tgt);
        }


        /// <summary>
        /// Compute how many points the units worth for
        /// the player
        /// </summary>
        /// <returns></returns>
        public override int getPoints()
        {            
            ICase targetType = Game.Instance.Map.getCase(this.CurrentPosition);

            if (targetType is Forest)
                return base.getPoints() + 1;
            else if (targetType is Plain)
                return 0;
            else
                return base.getPoints();
        }

    }
}
