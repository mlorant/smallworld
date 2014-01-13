using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SmallWorld
{
    [Serializable()]
    public class Dwarf : Unit, IDwarf
    {

        public Dwarf(int id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Constructor for the deserialization
        /// </summary>
        public Dwarf(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }


        /// <summary>
        /// Check if the unit can move to the position given.
        /// Dwarfs can move on any mountain without ennemies if
        /// they're on a mountain already.
        /// </summary>
        /// <param name="tgt"></param>
        /// <returns></returns>

        public override bool canMoveOn(Point tgt)
        {
            ICase destType = Game.Instance.Map.getCase(tgt);
            ICase currentType = Game.Instance.Map.getCase(this.CurrentPosition);

            if(currentType is Mountain && destType is Mountain && this.MovePoint > 0) 
            {
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
