using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    /// <summary>
    /// Small game builder, for quick game.
    /// </summary>
    public class SmallGameBuilder : GameCreation, ISmallGameBuiler
    {
        /// <summary>
        /// Maximal number of round possible in a game.
        /// </summary>
        public override int NB_ROUNDS
        {
            get { return 20; }
        }

        /// <summary>
        /// Side size of the map (in number of tiles)
        /// </summary>
        public override int MAP_SIZE
        {
            get { return 10; }
        }

        /// <summary>
        /// Number of the units for each player at the beginning of the game.
        /// </summary>
        public override int NB_UNITS
        {
            get { return 6; }
        }
    }
}
