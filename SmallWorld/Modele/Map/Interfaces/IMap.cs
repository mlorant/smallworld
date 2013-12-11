using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IMap
    {

        /// <summary>
        /// Return the tile type of the position given
        /// </summary>
        /// <param name="pos">Position to retrieve</param>
        /// <returns></returns>
        ICase getCase(Point pos);

        /// <summary>
        /// Returns the best defensive unit on the tile, which will
        /// be used to fight in the battle.
        /// </summary>
        /// <param name="index">Index of the tile</param>
        /// <returns>The best defensive unit of the tile</returns>
        IUnit getBestDefensiveUnit(Point pos);
    }
}
