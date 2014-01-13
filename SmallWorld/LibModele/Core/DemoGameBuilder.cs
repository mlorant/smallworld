using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{

    /// <summary>
    /// Game builder for demonstration. Adapt attributes 
    /// to creates small game in term of time
    /// </summary>
    public class DemoGameBuilder : GameCreation, IDemoGameBuilder
    {
        /// <summary>
        /// Maximal number of round possible in a game.
        /// </summary>
        public override int NB_ROUNDS
        {
            get { return 5; }
        }

        /// <summary>
        /// Side size of the map (in number of tiles)
        /// </summary>
        public override int MAP_SIZE
        {
            get { return 5; }
        }

        /// <summary>
        /// Number of the units for each player at the beginning of the game.
        /// </summary>
        public override int NB_UNITS
        {
            get { return 4; }
        }
    }
}
