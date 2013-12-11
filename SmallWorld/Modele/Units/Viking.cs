using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class Viking : Unit, IViking
    {

        /// <summary>
        /// Check if the unit can move to the position given,
        /// especially on a sea.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool canMoveOn(Point tgt)
        {
            return this.isNear(tgt);
        }
    }
}
