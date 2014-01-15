using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SmallWorld
{

    /// <summary>
    /// Gallic unit representation.
    /// Move faster on plains
    /// </summary>
    [Serializable()]
    public class Gallic : Unit, IGallic
    {
        public Gallic(int id)
        {
            this.Id = id;
        }

        public Gallic(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }

        /// <summary>
        /// La méthode qui permet au unité de bouger de case. Prend en compte l'autorisation ou non de bouger
        /// Les Gaulois ont le droit de bouger de 2 cases "plaine".
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool move(Point target)
        {
            bool ret = base.move(target);
            if (ret)
            {
                if (Game.Instance.Map.getCase(target) is Plain)
                {
                    this.MovePoint += 0.5;
                }
            }
            return ret;
        }


        /// <summary>
        /// Compute how many points the units worth for
        /// the player
        /// </summary>
        /// <returns></returns>
        public override int getPoints()
        {
            ICase targetType = Game.Instance.Map.getCase(this.CurrentPosition);

            if (targetType is Plain)
                return base.getPoints() + 1;
            else if (targetType is Mountain)
                return 0;
            else
                return base.getPoints();
        }

        /// <summary>
        /// Check if the unit can move to the position given.
        /// By default, only if the unit is near the tile and if
        /// it isn't to a Sea tile.
        /// </summary>
        /// <param name="tgt"></param>
        /// <returns></returns>
        public override bool canMoveOn(Point tgt)
        {
            ICase targetType = Game.Instance.Map.getCase(tgt);
            // Elle doit avoir le droit de bouger
            if (this.MovePoint >= 1 || (targetType is Plain && this.MovePoint >= 0.5))
            {         
                return this.isNear(tgt) && !(targetType is Sea);
            }
            return false;
        }

    }
}
