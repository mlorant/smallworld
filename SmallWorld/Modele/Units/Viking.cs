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
        /// Check if the unit can move to the position given,
        /// especially on a sea.
        /// </summary>
        /// <param name="tgt"></param>
        /// <returns></returns>
        public new bool canMoveOn(Point tgt)
        {
            return this.isNear(tgt);
        }
    }
}
