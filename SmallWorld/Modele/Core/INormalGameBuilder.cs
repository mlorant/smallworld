using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    /// <summary>
    /// Normal Game builder, with classic rules.
    /// </summary>
    public interface INormalGameBuilder : IGameCreation
    {
        /// <summary>
        /// Maximal number of round possible in a game.
        /// </summary>
        public override int NB_ROUNDS
        {
            get;
        }

        /// <summary>
        /// Side size of the map (in number of tiles)
        /// </summary>
        public override int MAP_SIZE
        {
            get;
        }

        /// <summary>
        /// Number of the units for each player at the beginning of the game.
        /// </summary>
        public override int NB_UNITS
        {
            get;
        }
    }
}
